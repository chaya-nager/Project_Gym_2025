using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interfaces;

namespace Service.Services
{
    public class CreateWorkoutPlan
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WorkoutVideo> videoRepository;
        private readonly IRepository<UserWorkoutPlan> planRepository;

        public CreateWorkoutPlan(IRepository<User> userRepository, IRepository<WorkoutVideo> videoRepository, IRepository<UserWorkoutPlan> planRepository)
        {
            this.userRepository = userRepository;
            this.videoRepository = videoRepository;
            this.planRepository = planRepository;
        }

        public async Task<UserWorkoutPlan> GenerateWorkoutPlan(int userId, int desiredDuration, string difficultyLevel)
        {
            var user = await userRepository.GetByIdAsync(userId);
            var allVideos = await videoRepository.GetAllAsync();

            string healthConditions = user.HealthConditions ?? "";
            DateTime minBirthDate = DateTime.Today.AddYears(-13);

            // שלב 1: סינון כללי לפי קושי, בריאות, גיל וכו'
            var filteredVideos = allVideos.Where(v =>
                v.DifficultyLevel == difficultyLevel &&
                (string.IsNullOrEmpty(v.TargetAudience) ||
                 !healthConditions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Any(cond => v.TargetAudience.Contains(cond.Trim(), StringComparison.OrdinalIgnoreCase))) &&
                (user.BirthDate <= minBirthDate || (v.TargetAudience?.Contains("ילדים") ?? false))
            ).ToList();

            // שלב 2: חלוקה לפי שלב האימון לפי מילות מפתח בתיאור
            var warmupVideos = filteredVideos.Where(v => v.Description.Contains("חימום", StringComparison.OrdinalIgnoreCase)).ToList();
            var cooldownVideos = filteredVideos.Where(v => v.Description.Contains("מתיחות", StringComparison.OrdinalIgnoreCase)).ToList();

            // אימונים עיקריים הם כל השאר שלא חימום או מתיחות
            var mainVideos = filteredVideos.Except(warmupVideos).Except(cooldownVideos).ToList();

            // שלב 3: יצירת כל קומבינציות אפשריות של חימום + אימון + מתיחות
            // נניח לפחות 1 סרטון מכל שלב
            var bestCombination = new List<WorkoutVideo>();
            double bestScore = double.NegativeInfinity;

            foreach (var warmup in warmupVideos)
            {
                // אפשר גם לבחור יותר מחימום אחד אם רוצים, כאן נניח אחד
                foreach (var cooldown in cooldownVideos)
                {
                    // כאן נרצה לבדוק כל תת-קבוצה של סרטוני אימון מרכזיים (power set)
                    var mainSubsets = GetAllSubsets(mainVideos);

                    foreach (var mainSet in mainSubsets)
                    {
                        if (mainSet.Count == 0) continue; // לפחות אחד אימון

                        var candidate = new List<WorkoutVideo>();
                        candidate.Add(warmup);
                        candidate.AddRange(mainSet);
                        candidate.Add(cooldown);

                        int totalDuration = candidate.Sum(v => v.Duration);
                        if (totalDuration <= desiredDuration)
                        {
                            double score = CalculateCombinationScore(candidate, user, desiredDuration, difficultyLevel);
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestCombination = candidate;
                            }
                        }
                    }
                }
            }

            var plan = new UserWorkoutPlan
            {
                UserId = user.UserId,
                WorkoutPlanVideos = bestCombination
            };

            await planRepository.AddItemAsync(plan);
            return plan;
        }

        // פונקציה מחזירה את כל תת-הקבוצות של רשימת סרטונים (Power set)
        private List<List<WorkoutVideo>> GetAllSubsets(List<WorkoutVideo> videos)
        {
            var subsets = new List<List<WorkoutVideo>>();
            int subsetCount = 1 << videos.Count; // 2^n

            for (int i = 1; i < subsetCount; i++) // מתחילים מ-1 כדי למנוע קבוצה ריקה
            {
                var subset = new List<WorkoutVideo>();
                for (int j = 0; j < videos.Count; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        subset.Add(videos[j]);
                    }
                }
                subsets.Add(subset);
            }
            return subsets;
        }

        // חישוב ניקוד של קומבינציה לפי קריטריונים שונים
        private double CalculateCombinationScore(List<WorkoutVideo> videos, User user, int desiredDuration, string difficultyLevel)
        {
            double score = 0;

            int totalDuration = videos.Sum(v => v.Duration);

            // ניקוד קרוב למשך הרצוי
            score += 1 - (Math.Abs(totalDuration - desiredDuration) / (double)desiredDuration);

            // ניקוד לפי רמת קושי
            bool allMatchDifficulty = videos.All(v => v.DifficultyLevel == difficultyLevel);
            if (allMatchDifficulty)
                score += 1;
            else
                score += 0.5;

            // עונש אם סרטון לא מתאים למצב בריאות
            if (!string.IsNullOrEmpty(user.HealthConditions))
            {
                foreach (var v in videos)
                {
                    if (!string.IsNullOrEmpty(v.TargetAudience) &&
                        user.HealthConditions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Any(cond => v.TargetAudience.Contains(cond.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        score -= 1;
                    }
                }
            }

            // אפשר להוסיף שיקולים נוספים לפי צורך

            return score;
        }
    }
}
