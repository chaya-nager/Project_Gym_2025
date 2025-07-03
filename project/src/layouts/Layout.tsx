import { NavLink, useNavigate, Outlet } from "react-router-dom";
import { removeSession } from "../auth/auth.utils";
import { RoleType } from "../types/user.types";
import RoleGuard from "../auth/RoleGuard";
import { Paths } from "../routes/paths";
import { isLoggedOut } from "../auth/auth.utils";

export const Layout = () => {
  return (
    <>
      <header style={headerStyle}>
        <NavBar />
        <AccountButton />
      </header>
      <main style={mainStyle}>
        <Outlet />
      </main>
    </>
  );
};

export const NavBar = () => {
  return (
    <nav style={navStyle}>
      <NavLink to={Paths.home} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
        עמוד הבית
      </NavLink>
      <NavLink to={Paths.login} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
        התחברות
      </NavLink>
      <NavLink to={Paths.signUp} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
        הרשמה
      </NavLink>

      <RoleGuard roles={[RoleType.User]}>
        <NavLink to={Paths.choosePlan} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
          בקשת סרטון
        </NavLink>
      </RoleGuard>

      <RoleGuard roles={[RoleType.Trainer]}>
        <NavLink to={Paths.uploadVideo} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
          העלאת סרטון
        </NavLink>
        <NavLink to={Paths.myTrainerVideos} style={linkStyle} className={({ isActive }) => (isActive ? activeLink : "")}>
          הסרטונים שלי
        </NavLink>
      </RoleGuard>
    </nav>
  );
};

const AccountButton = () => {
  const navigate = useNavigate();

  return (
    <button style={logoutBtnStyle} onClick={() => navigate(Paths.logout)}>
      התנתקות
    </button>
  );
};

// --- עיצובים
const headerStyle: React.CSSProperties = {
  backgroundColor: "#ffffff",
  padding: "1rem 2rem",
  display: "flex",
  justifyContent: "space-between",
  alignItems: "center",
  boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)",
};

const mainStyle: React.CSSProperties = {
  padding: "2rem",
  backgroundColor: "#f9f9f9",
  minHeight: "calc(100vh - 80px)",
};

const navStyle: React.CSSProperties = {
  display: "flex",
  gap: "1.5rem",
  alignItems: "center",
};

const linkStyle: React.CSSProperties = {
  color: "#555",
  fontWeight: "bold",
  textDecoration: "none",
  fontSize: "1rem",
};

const activeLink = "active";

const logoutBtnStyle: React.CSSProperties = {
  backgroundColor: "#e53935",
  color: "white",
  border: "none",
  padding: "0.6rem 1.2rem",
  borderRadius: "8px",
  cursor: "pointer",
  fontWeight: "bold",
};
