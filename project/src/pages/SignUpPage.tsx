import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export const SignUpPage = () => {
  const navigate = useNavigate();
  const [isSuccess, setIsSuccess] = useState(false);
  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    passwordHash: "",
    birthDate: "",
    userType: "User",
    healthConditions: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post("https://localhost:7286/api/user", formData);
      setIsSuccess(true);
    } catch (error) {
      console.error(error);
      alert("שגיאה בהרשמה");
    }
  };

  useEffect(() => {
    if (isSuccess) {
      setTimeout(() => {
        navigate("/auth/login");
      }, 2000);
    }
  }, [isSuccess]);

  return (
    <div style={wrapperStyle}>
      <form onSubmit={handleSubmit} style={formStyle}>
        <h2 style={titleStyle}>הרשמה</h2>
        <input
          name="fullName"
          placeholder="שם מלא"
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          name="email"
          placeholder="אימייל"
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          name="passwordHash"
          type="password"
          placeholder="סיסמה"
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          name="birthDate"
          type="date"
          onChange={handleChange}
          style={inputStyle}
        />
        <select name="userType" onChange={handleChange} style={inputStyle}>
          <option value="User">מתאמן</option>
          <option value="Trainer">מאמן</option>
        </select>
        <input
          name="healthConditions"
          placeholder="מצב בריאותי (לא חובה)"
          onChange={handleChange}
          style={inputStyle}
        />

        <button
          type="submit"
          disabled={isSuccess}
          style={{
            padding: "0.75rem",
            borderRadius: "8px",
            border: "none",
            backgroundColor: isSuccess ? "#4caf50" : "#2196f3",
            color: "white",
            fontWeight: "bold",
            fontSize: "1rem",
            cursor: isSuccess ? "default" : "pointer",
          }}
        >
          {isSuccess ? "✔ נרשמת בהצלחה!" : "הרשמה"}
        </button>
      </form>
    </div>
  );
};


// סגנונות משותפים
const wrapperStyle = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  height: "100vh",
  background: "linear-gradient(135deg, #e0f7fa, #e8f5e9)"
}as const;

const formStyle = {
  display: "flex",
  flexDirection: "column",
  gap: "1rem",
  padding: "2rem",
  borderRadius: "12px",
  backgroundColor: "#ffffff",
  boxShadow: "0 8px 24px rgba(0,0,0,0.1)",
  width: "100%",
  maxWidth: "400px"
}as const;

const titleStyle = {
  textAlign: "center",
  color: "#1976d2"
}as const;

const inputStyle = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "1px solid #ccc",
  fontSize: "1rem"
}as const;

const buttonStyle = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "none",
  backgroundColor: "#4caf50",
  color: "white",
  fontWeight: "bold",
  fontSize: "1rem",
  cursor: "pointer"
}as const;
