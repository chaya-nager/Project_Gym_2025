import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

// פונקציית עזר לפענוח JWT
function parseJwt(token: string) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64).split('').map(c =>
        '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join('')
    );
    return JSON.parse(jsonPayload);
  } catch {
    return null;
  }
}

const UploadVideoPage = () => {
  const navigate = useNavigate();
  const [success, setSuccess] = useState(false);

  const [videoData, setVideoData] = useState({
    title: '',
    description: '',
    duration: '',
    difficultyLevel: 'Beginner',
    workoutType: '',
    targetAudience: '',
    audienceAgeGroup: '', // ✅ חדש
  });

  const [file, setFile] = useState<File | null>(null);

  useEffect(() => {
    const token = sessionStorage.getItem("token");
    if (!token) {
      navigate("/home");
      return;
    }
    const decoded = parseJwt(token);
    const role = decoded?.role || decoded?.UserType ||
      decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    if (role?.toLowerCase() !== "trainer") {
      navigate("/home");
    }
  }, [navigate]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setVideoData({ ...videoData, [e.target.name]: e.target.value });
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files?.length) {
      setFile(e.target.files[0]);
    }
  };

  const handleUpload = async () => {
    if (!file) {
      alert("יש לבחור קובץ");
      return;
    }

    const token = sessionStorage.getItem("token");
    if (!token) {
      alert("אין טוקן התחברות");
      return;
    }

    const decoded = parseJwt(token);
    const trainerId = decoded?.nameid || decoded?.TrainerId || decoded?.UserId;
    if (!trainerId || isNaN(trainerId)) {
      alert("TrainerId לא תקין");
      return;
    }

    const formData = new FormData();
    formData.append("Title", videoData.title);
    formData.append("Description", videoData.description);
    formData.append("Duration", parseInt(videoData.duration).toString());
    formData.append("DifficultyLevel", videoData.difficultyLevel);
    formData.append("WorkoutType", videoData.workoutType);
    formData.append("TargetAudience", videoData.targetAudience);
    formData.append("AudienceAgeGroup", videoData.audienceAgeGroup); // ✅ חדש
    formData.append("fileVideo", file);
    formData.append("TrainerId", trainerId.toString());

    try {
      await axios.post("https://localhost:7286/api/WorkoutVideo", formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data"
        }
      });
      setSuccess(true);
    } catch (err) {
      console.error(err);
      alert("שגיאה בהעלאה");
    }
  };

  if (success) {
    return (
      <div style={successStyle}>
        <h2 style={titleStyle}>✅ הסרטון הועלה בהצלחה!</h2>
        <button style={buttonStyle} onClick={() => {
          setSuccess(false);
          setVideoData({
            title: '',
            description: '',
            duration: '',
            difficultyLevel: 'Beginner',
            workoutType: '',
            targetAudience: '',
            audienceAgeGroup: '', // איפוס
          });
          setFile(null);
        }}>
          העלאת סרטון נוסף
        </button>
        <button style={{ ...buttonStyle, backgroundColor: "#1976d2" }} onClick={() => navigate("/")}>
          חזרה לעמוד הבית
        </button>
      </div>
    );
  }

  return (
    <div style={wrapperStyle}>
      <form onSubmit={(e) => { e.preventDefault(); handleUpload(); }} style={formStyle}>
        <h2 style={titleStyle}>העלאת סרטון</h2>

        <input name="title" placeholder="כותרת" onChange={handleChange} style={inputStyle} />
        <textarea name="description" placeholder="תיאור" onChange={handleChange} style={inputStyle} />
        <input name="duration" type="number" placeholder="משך בדקות" onChange={handleChange} style={inputStyle} />

        <select name="difficultyLevel" onChange={handleChange} style={inputStyle}>
          <option value="Beginner">מתחילים</option>
          <option value="Intermediate">בינוני</option>
          <option value="Advanced">מתקדמים</option>
        </select>

        <input name="workoutType" placeholder="סוג אימון" onChange={handleChange} style={inputStyle} />
        <input name="targetAudience" placeholder="מצב רפואי (לב, סכרת וכו')" onChange={handleChange} style={inputStyle} />

        <select name="audienceAgeGroup" value={videoData.audienceAgeGroup} onChange={handleChange} style={inputStyle}>
          <option value="">בחר קבוצת גיל</option>
          <option value="צעירים">צעירים</option>
          <option value="מבוגרים">מבוגרים</option>
          <option value="לכולם">לכולם</option>
        </select>

        <input type="file" accept="video/*" onChange={handleFileChange} style={inputStyle} />

        <button type="submit" style={buttonStyle}>העלה</button>
      </form>
    </div>
  );
};

export default UploadVideoPage;

const wrapperStyle: React.CSSProperties = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  height: "100vh",
  background: "#e0f7fa"
};

const formStyle: React.CSSProperties = {
  display: "flex",
  flexDirection: "column",
  gap: "1rem",
  padding: "2rem",
  borderRadius: "12px",
  backgroundColor: "#ffffff",
  boxShadow: "0 8px 24px rgba(0,0,0,0.1)",
  width: "100%",
  maxWidth: "400px"
};

const titleStyle: React.CSSProperties = {
  textAlign: "center",
  color: "#1976d2"
};

const inputStyle: React.CSSProperties = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "1px solid #ccc",
  fontSize: "1rem"
};

const buttonStyle: React.CSSProperties = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "none",
  backgroundColor: "#4caf50",
  color: "white",
  fontWeight: "bold",
  fontSize: "1rem",
  cursor: "pointer"
};

const successStyle: React.CSSProperties = {
  textAlign: "center",
  padding: "2rem",
  backgroundColor: "#e8f5e9",
  borderRadius: "12px",
  boxShadow: "0 4px 12px rgba(0,0,0,0.1)"
};
