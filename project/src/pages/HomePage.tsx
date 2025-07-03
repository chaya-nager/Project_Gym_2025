import React from "react";

const HomePage = () => {
  return (
    <div style={wrapperStyle}>
      <div style={heroSection}>
        <h1 style={heroTitle}>התעמלות בכיף Gym -ברוך הבא ל</h1>
        <p style={heroSubtitle}>האימונים שלנו, ההצלחה שלך.</p>
        <p style={heroText}>
          המערכת המתקדמת בישראל לאימוני כושר ביתיים, עם תוכניות מותאמות אישית, מאמנים מקצועיים וסרטונים באיכות גבוהה.
        </p>
      </div>

      <div style={featuresContainer}>
        <div style={featureBox}>
          <h3 style={featureTitle}>תוכנית מותאמת אישית</h3>
          <p style={featureText}>בחר סוג אימון, רמה ואנחנו נבנה לך את המסלול המדויק.</p>
        </div>
        <div style={featureBox}>
          <h3 style={featureTitle}>מאמנים מוסמכים</h3>
          <p style={featureText}>כל סרטון מופק ע"י מאמן מקצועי עם תעודות וניסיון.</p>
        </div>
        <div style={featureBox}>
          <h3 style={featureTitle}>גישה מכל מקום</h3>
          <p style={featureText}>אתר רספונסיבי שניתן לצפות בו מהמחשב או הנייד.</p>
        </div>
      </div>
    </div>
  );
};

const wrapperStyle = {
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  padding: "3rem 2rem",
  background: "linear-gradient(135deg, #f0f4ff, #e8f5e9)",
  minHeight: "100vh",
  fontFamily: "sans-serif"
} as const;

const heroSection = {
  textAlign: "center",
  marginBottom: "4rem",
  maxWidth: "800px"
} as const;

const heroTitle = {
  fontSize: "3rem",
  color: "#2c387e",
  marginBottom: "1rem"
} as const;

const heroSubtitle = {
  fontSize: "1.5rem",
  color: "#1976d2",
  marginBottom: "1rem"
} as const;

const heroText = {
  fontSize: "1.1rem",
  color: "#444"
} as const;

const featuresContainer = {
  display: "flex",
  flexWrap: "wrap",
  gap: "1.5rem",
  justifyContent: "center"
} as const;

const featureBox = {
  backgroundColor: "#fff",
  borderRadius: "1rem",
  padding: "1.5rem",
  width: "260px",
  boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
  textAlign: "center"
} as const;

const featureTitle = {
  fontSize: "1.25rem",
  fontWeight: "bold",
  color: "#4a148c",
  marginBottom: "0.5rem"
} as const;

const featureText = {
  fontSize: "0.95rem",
  color: "#555"
} as const;

export default HomePage;
