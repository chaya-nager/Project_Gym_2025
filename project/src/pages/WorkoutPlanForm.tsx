import React, { useState } from "react";
import axios from "axios";
import { CSSProperties } from "react";
import { useNavigate } from "react-router-dom";

const WorkoutPlanForm = () => {
  const [formData, setFormData] = useState({
    difficulty: "",
    workoutType: "",
    desiredDuration: 20,
    includeWarmup: false,
    includeCooldown: false,
  });

  const [videos, setVideos] = useState<any[]>([]);
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const target = e.target as HTMLInputElement;
    const { name, value, type, checked } = target;
    const updatedValue = type === "checkbox" ? checked : value;
    setFormData({ ...formData, [name]: updatedValue });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const token = sessionStorage.getItem("token");
    const userData = sessionStorage.getItem("user");

    if (!token || !userData) {
      alert("לא נמצא טוקן או נתוני משתמש. התחברי מחדש.");
      return;
    }

    const user = JSON.parse(userData);
    const userId = user?.id;
    const targetAudience = user?.medicalCondition || "";

    if (!userId) {
      alert("לא נמצא מזהה משתמש.");
      return;
    }

    try {
      const payload = {
        userId,
        desiredDuration: parseInt(formData.desiredDuration.toString(), 10),
        difficultyLevel: formData.difficulty,
        workoutType: formData.workoutType,
        targetAudience: targetAudience,
        includeWarmup: formData.includeWarmup,
        includeCooldown: formData.includeCooldown,
      };

      const response = await axios.post(
        "https://localhost:7286/api/CreateWorkoutPlan/generate",
        payload,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      const data = response.data;
      const videosArray = Array.isArray(data.workoutPlanVideos)
        ? data.workoutPlanVideos
        : data.workoutPlanVideos?.$values || [];

      setVideos(videosArray);
      setMessage(videosArray.length ? "" : "לא נמצאה תוכנית אימון מתאימה. ניתן לשנות את הסינון ולנסות שוב.");
    } catch (error) {
      console.error("שגיאה בבקשה לשרת:", error);
      alert("שגיאה בשליחה לשרת.");
    }
  };

  return (
    <div style={styles.wrapper}>
      <form style={styles.form} onSubmit={handleSubmit}>
        <h2 style={styles.title}>חיפוש סרטוני אימון</h2>

        <label>רמת קושי:</label>
        <select name="difficulty" value={formData.difficulty} onChange={handleChange} style={styles.input}>
          <option value="">בחר רמת קושי</option>
          <option value="Beginner">מתחילים</option>
          <option value="Intermediate">ביניים</option>
          <option value="Advanced">מתקדמים</option>
        </select>

        <label>סוג אימון:</label>
        <select name="workoutType" value={formData.workoutType} onChange={handleChange} style={styles.input}>
          <option value="">בחר סוג אימון</option>
          <option value="ארובי">ארובי</option>
          <option value="כוח">כוח</option>
          <option value="שיווי משקל וקואורדינציה">שיווי משקל וקואורדינציה</option>
          <option value="אינטרוולים">אינטרוולים</option>
          <option value="התעמלות טיפולית / שיקומית">התעמלות טיפולית / שיקומית</option>
        </select>

        <label>משך אימון (בדקות):</label>
        <input
          type="number"
          name="desiredDuration"
          value={formData.desiredDuration}
          onChange={handleChange}
          style={styles.input}
        />

        <label>
          <input type="checkbox" name="includeWarmup" checked={formData.includeWarmup} onChange={handleChange} style={{ marginRight: "0.5rem" }} />
          כלול חימום
        </label>

        <label>
          <input type="checkbox" name="includeCooldown" checked={formData.includeCooldown} onChange={handleChange} style={{ marginRight: "0.5rem" }} />
          כלול מתיחות
        </label>

        <button type="submit" style={styles.button}>חפש סרטונים</button>
      </form>

      {message && <div style={{ marginTop: "2rem", color: "#e53935", fontWeight: "bold" }}>{message}</div>}

      {videos.length > 0 && (
        <div style={{ marginTop: "2rem", width: "100%", maxWidth: "800px" }}>
          <h3 style={styles.title}>תוצאות:</h3>
          {videos.map((video) => (
            <div key={`video-${video.videoId || Math.random()}`} style={styles.videoCard}>
              <h4>{video.title}</h4>
              <p>{video.description}</p>
              <p>⏱ משך: {video.duration} דקות</p>
              <p>סוג: {video.workoutType}</p>
              <button
                style={{
                  marginTop: "1rem",
                  padding: "0.5rem 1rem",
                  backgroundColor: "#007bff",
                  color: "#fff",
                  border: "none",
                  borderRadius: "6px",
                  cursor: "pointer",
                }}
                onClick={() => {
                  if (typeof video.videoUrl === "string" && video.videoUrl.trim().startsWith("http")) {
                    window.open(video.videoUrl, "_blank");
                  } else {
                    alert("קובץ הווידאו אינו זמין או לא תקין.");
                  }
                }}
              >
                צפייה בדף נפרד
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default WorkoutPlanForm;

const styles: { [key: string]: CSSProperties } = {
  wrapper: {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    padding: "2rem",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1rem",
    padding: "2rem",
    backgroundColor: "#f3f3f3",
    borderRadius: "12px",
    boxShadow: "0 0 10px rgba(0,0,0,0.1)",
    width: "100%",
    maxWidth: "400px",
  },
  title: {
    textAlign: "center",
    color: "#007bff",
  },
  input: {
    padding: "0.5rem",
    fontSize: "1rem",
    borderRadius: "6px",
    border: "1px solid #ccc",
  },
  button: {
    padding: "0.7rem",
    fontSize: "1rem",
    backgroundColor: "#28a745",
    color: "#fff",
    border: "none",
    borderRadius: "6px",
    cursor: "pointer",
  },
  videoCard: {
    backgroundColor: "#fff",
    padding: "1rem",
    marginBottom: "1rem",
    borderRadius: "8px",
    boxShadow: "0 0 5px rgba(0,0,0,0.1)",
  },
};
