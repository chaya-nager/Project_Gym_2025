import React, { useEffect, useState } from "react";
import axios from "axios";

interface UserWorkoutPlanDto {
  id: number;
  userId: number;
  videoIds: number[];
}

interface WorkoutVideoDto {
  videoId: number;
  title: string;
  videoUrl: string;
}

const WorkoutPlanPage: React.FC = () => {
  const [plan, setPlan] = useState<UserWorkoutPlanDto | null>(null);
  const [videos, setVideos] = useState<WorkoutVideoDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios.get(`/api/UserWorkoutPlan/13`)
      .then((res) => {
        console.log("📦 תכנית שהתקבלה:", res.data);
        setPlan(res.data);
        return res.data.videoIds;
      })
      .then((ids: number[]) => {
        return axios.get("/api/WorkoutVideo");
      })
      .then((res) => {
        console.log("🎬 סרטונים שהתקבלו:", res.data);
        setVideos(res.data);
      })
      .catch((err) => {
        console.error("❌ שגיאה בטעינה:", err);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  const filteredVideos = plan?.videoIds?.length
    ? videos.filter((v) => plan.videoIds.includes(v.videoId))
    : [];

  if (loading) return <p>טוען...</p>;

  return (
    <div style={{ padding: "1rem", direction: "rtl" }}>
      <h1>📝 תכנית אימון מספר {plan?.id}</h1>
      <h2>🎬 סרטונים בתכנית:</h2>
      {Array.isArray(filteredVideos) && filteredVideos.length > 0 ? (
        filteredVideos.map((video) => (
          <div key={video.videoId} style={{ marginBottom: "20px" }}>
            <h3>{video.title}</h3>
            <video
              width="500"
              controls
              src={`/Videos/${video.videoUrl}`}
              style={{ border: "1px solid #ccc" }}
            />
          </div>
        ))
      ) : (
        <p>אין סרטונים</p>
      )}
    </div>
  );
};

export default WorkoutPlanPage;
