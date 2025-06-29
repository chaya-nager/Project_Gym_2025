import { useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export const LoginPage = () => {
    const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    email: '',
    userName: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  };

  const handleLogin = async () => {
    try {
      const formData = new FormData();
      formData.append("UserName", credentials.userName);
      formData.append("Email", credentials.email);

      const res = await axios.post("https://localhost:7286/api/login/login", formData);
      const token = res.data;
      if (token === "Unauthorized") {
        alert("פרטי התחברות שגויים");
        return;
      }

      localStorage.setItem("token", token);
      navigate("/home"); 
    } catch (err) {
      console.error(err);
      alert("שגיאה בהתחברות");
    }
  };

  return (
    <div style={wrapperStyle}>
      <form onSubmit={(e) => { e.preventDefault(); handleLogin(); }} style={formStyle}>
        <h2 style={titleStyle}>התחברות</h2>
        <input name="email" placeholder="אימייל" onChange={handleChange} style={inputStyle} />
        <input name="userName" placeholder="שם מלא" onChange={handleChange} style={inputStyle} />
        <button type="submit" style={buttonStyle}>התחבר</button>
        <p style={{ textAlign: "center" }}>
          עדיין לא רשום? <Link to="/auth/sign-up" style={{ color: "#ff9800" }}>הרשמה</Link>
        </p>
      </form>
    </div>
  );
};

// סגנונות זהים
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
