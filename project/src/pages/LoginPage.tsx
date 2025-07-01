import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import axios from "axios";
import { useAppDispatch } from "../redux/store";
import { setAuth,setInitialized  } from "../redux/auth/auth.slice";
import { RoleType } from "../types/user.types";

// פענוח JWT
function parseJwt(token: string) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
}

const LoginPage = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const [credentials, setCredentials] = useState({
    UserName: '',
    Email: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  };

  const handleLogin = async () => {
    try {
      const formData = new FormData();
      formData.append("UserName", credentials.UserName);
      formData.append("Email", credentials.Email);

      const res = await axios.post("https://localhost:7286/api/login/login", formData);
      const token = res.data;

      if (token === "Unauthorized") {
        alert("פרטי התחברות שגויים");
        return;
      }

      const decoded = parseJwt(token);
      const rawRole = decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        || decoded?.role || decoded?.UserType;

      const role = (rawRole as string)?.toLowerCase() as RoleType;

      const user = {
        id: decoded?.UserId || 0,
        name: decoded?.FullName || "",
        email: decoded?.Email || "",
        phone: "",
        address: "",
        role: role?.toLowerCase() as RoleType
      };

      localStorage.setItem("token", token);

      dispatch(setAuth(user));
      dispatch(setInitialized());
      console.log("✅ התחברת כ:", role);
      setTimeout(() => {
        if (role === RoleType.Trainer) {
          navigate("/upload-video", { replace: true });
        } else {
          navigate("/choose-plan", { replace: true });
        }
      }, 50);
      
    } catch (err) {
      console.error(err);
      alert("שגיאה בהתחברות");
    }
  };

  return (
    <div style={wrapperStyle}>
      <form onSubmit={(e) => { e.preventDefault(); handleLogin(); }} style={formStyle}>
        <h2 style={titleStyle}>התחברות</h2>
        <input name="UserName" placeholder="שם משתמש" onChange={handleChange} style={inputStyle} />
        <input name="Email" placeholder="אימייל" onChange={handleChange} style={inputStyle} />
        <button type="submit" style={buttonStyle}>התחבר</button>
        <p style={{ textAlign: "center" }}>
          עדיין לא רשום? <Link to="/auth/sign-up" style={{ color: "#ff9800" }}>הרשמה</Link>
        </p>
      </form>
    </div>
  );
};

export default LoginPage;

// עיצוב
const wrapperStyle = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  height: "100vh",
  background: "linear-gradient(135deg, #e0f7fa, #e8f5e9)"
} as const;

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
} as const;

const titleStyle = {
  textAlign: "center",
  color: "#1976d2"
} as const;

const inputStyle = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "1px solid #ccc",
  fontSize: "1rem"
} as const;

const buttonStyle = {
  padding: "0.75rem",
  borderRadius: "8px",
  border: "none",
  backgroundColor: "#4caf50",
  color: "white",
  fontWeight: "bold",
  fontSize: "1rem",
  cursor: "pointer"
} as const;