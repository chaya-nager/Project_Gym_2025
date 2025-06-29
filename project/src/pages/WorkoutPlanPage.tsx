import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

const WorkoutPlanPage = () => {
  const { id } = useParams(); // מזהה תוכנית האימון מתוך ה-URL
  const [plan, setPlan] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchWorkoutPlan = async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(`https://localhost:7286/api/WorkoutPlan/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        setPlan(response.data);
      } catch (error) {
        console.error("שגיאה בטעינת תוכנית האימון", error);
      } finally {
        setLoading(false);
      }
    };

    fetchWorkoutPlan();
  }, [id]);

  if (loading) return <div>טוען...</div>;
  if (!plan) return <div>לא נמצאה תוכנית אימון</div>;

  return (
    <div style={{ padding: "2rem" }}>
      <h1>תוכנית אימון #{plan.id}</h1>
      <p>שם משתמש: {plan.user?.fullName}</p>
      <h2>סרטונים:</h2>
      {plan.workoutPlanVideos.length === 0 ? (
        <p>אין סרטונים בתוכנית זו</p>
      ) : (
        <ul>
          {plan.workoutPlanVideos.map((video: any) => (
            <li key={video.videoId}>
              <strong>{video.title}</strong> – {video.duration} דקות, רמה: {video.difficultyLevel}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default WorkoutPlanPage;
