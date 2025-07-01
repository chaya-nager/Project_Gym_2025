import React, { useState } from "react";
import axios from "axios";
import { CSSProperties } from "react";

const WorkoutPlanForm = () => {
  const [formData, setFormData] = useState({
    difficulty: "",
    workoutType: "",
    targetAudience: "",
  });

  const [videos, setVideos] = useState<any[]>([]);

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.get("https://localhost:7286/api/WorkoutVideo");

      const data = Array.isArray(response.data) ? response.data : [response.data];

      setVideos(data); // ⬅️ תוקן כאן

    } catch (error) {
      console.error("שגיאה בקבלת תוכניות:", error);
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
          <option value="אירובי">אירובי</option>
          <option value="כוח">כוח</option>
        </select>

        <label>קהל יעד:</label>
        <select name="targetAudience" value={formData.targetAudience} onChange={handleChange} style={styles.input}>
          <option value="">בחר קהל יעד</option>
          <option value="צעירים">צעירים</option>
          <option value="מבוגרים">מבוגרים</option>
        </select>

        <button type="submit" style={styles.button}>חפש סרטונים</button>
      </form>

      {videos.length > 0 && (
        <div style={{ marginTop: "2rem", width: "100%", maxWidth: "800px" }}>
          <h3 style={styles.title}>תוצאות:</h3>
          {videos.map((video) => (
            <div key={video.videoId}>
             <h4>{video.title}</h4>
              <p>{video.description}</p>
              <video controls width="100%" style={{ borderRadius: "10px" }}>
                <source src={`https://localhost:7286/Videos/${video.videoUrl}`} type="video/mp4" />
                הדפדפן שלך לא תומך בניגון וידאו.
              </video>
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
