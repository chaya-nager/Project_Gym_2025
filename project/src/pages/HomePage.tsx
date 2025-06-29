import React from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const HomePage = () => {
  const navigate = useNavigate();

  const handleStartWorkout = async () => {
    try {
      const token = localStorage.getItem("token");

      const response = await axios.post(
        "https://localhost:7286/api/CreateWorkoutPlan/generate",
        {
          userId: 1, // שימי כאן את ה־UserId האמיתי של המשתמש
          desiredDuration: 30,
          difficultyLevel: "בינוני"
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
          }
        }
      );

      console.log("תוכנית אימון:", response.data);
      const planId = response.data.id;
      navigate(`/workout-plan/${planId}`);
    } catch (err) {
      console.error("שגיאה ביצירת תוכנית אימון", err);
      alert("שגיאה ביצירת תוכנית אימון");
    }
  };

  return (
    <div style={wrapperStyle}>
      <div style={bannerStyle}>
        <h1 style={titleStyle}>מצא את האימון שמתאים בדיוק לך</h1>
        <p style={subtitleStyle}>התאמן בבית, בקצב שלך</p>
      </div>

      <form style={filterFormStyle}>
        <select style={selectStyle}>
          <option>גיל</option>
          <option>צעיר</option>
          <option>מבוגר</option>
        </select>
        <select style={selectStyle}>
          <option>רמת קושי</option>
          <option>מתחיל</option>
          <option>בינוני</option>
          <option>מתקדם</option>
        </select>
        <select style={selectStyle}>
          <option>סוג אימון</option>
          <option>אירובי</option>
          <option>יוגה</option>
          <option>כוח</option>
        </select>
        <select style={selectStyle}>
          <option>משך זמן</option>
          <option>עד 15 דק</option>
          <option>עד 30 דק</option>
          <option>יותר מ-30 דק</option>
        </select>
        <button type="submit" style={buttonStyle}>חפש אימון</button>
      </form>

      <div style={videosContainerStyle}>
        <div style={videoCardStyle}>אימון כושר 1</div>
        <div style={videoCardStyle}>אימון כושר 2</div>
        <div style={videoCardStyle}>אימון כושר 3</div>
      </div>

      <button style={ctaButtonStyle} onClick={handleStartWorkout}>
        🚀 התחל את האימון הראשון שלך עכשיו
      </button>
    </div>
  );
};

const wrapperStyle = {
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  padding: "2rem",
  background: "linear-gradient(135deg, #e0f7fa, #e8f5e9)",
  minHeight: "100vh"
} as const;

const bannerStyle = {
  textAlign: "center",
  marginBottom: "2rem"
} as const;

const titleStyle = {
  fontSize: "2.5rem",
  color: "#1976d2"
} as const;

const subtitleStyle = {
  fontSize: "1.25rem",
  color: "#555"
} as const;

const filterFormStyle = {
  display: "flex",
  gap: "1rem",
  flexWrap: "wrap",
  justifyContent: "center",
  marginBottom: "2rem"
} as const;

const selectStyle = {
  padding: "0.5rem",
  borderRadius: "8px",
  border: "1px solid #ccc"
} as const;

const buttonStyle = {
  padding: "0.75rem 1.5rem",
  borderRadius: "8px",
  backgroundColor: "#4caf50",
  color: "white",
  border: "none",
  fontWeight: "bold",
  cursor: "pointer"
} as const;

const videosContainerStyle = {
  display: "flex",
  gap: "1rem",
  flexWrap: "wrap",
  justifyContent: "center",
  marginBottom: "2rem"
} as const;

const videoCardStyle = {
  width: "200px",
  height: "120px",
  backgroundColor: "#ffffff",
  borderRadius: "12px",
  boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  fontWeight: "bold",
  color: "#1976d2"
} as const;

const ctaButtonStyle = {
  padding: "1rem 2rem",
  fontSize: "1.2rem",
  backgroundColor: "#2196f3",
  color: "white",
  border: "none",
  borderRadius: "12px",
  fontWeight: "bold",
  cursor: "pointer"
} as const;

export default HomePage;
