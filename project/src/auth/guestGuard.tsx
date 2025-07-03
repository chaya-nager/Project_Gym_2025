import { ReactNode } from "react"
import { Navigate } from "react-router"
import { useAppSelector } from "../redux/store"
import { selectAuth } from "../redux/auth/auth.selector"
import { Paths } from "../routes/paths"

type Props = {
    children: ReactNode
}

const GuestGuard = ({ children }: Props) => {
    const { isAuthorized, isInitialized } = useAppSelector(selectAuth); // הוספתי isInitialized לבדיקה

    // ודא שהסטור מאותחל
    if (!isInitialized) {
        console.log("GuestGuard - Not initialized, rendering null or loading...");
        return null; // או רכיב טעינה
    }

    console.log("GuestGuard - Current isAuthorized:", isAuthorized); // הודעה זו חייבת להיות הראשונה בתנאי

    if (isAuthorized) {
        // אם המשתמש מחובר, הפנה אותו לדף הבית
        console.log("GuestGuard - User is now authorized, redirecting to home."); // שינוי בניסוח כדי להיות ברור יותר
        return <Navigate to={Paths.home} replace />;
    }

    // אם המשתמש אינו מורשה, תן לו לגשת לדפי האורח
    console.log("GuestGuard - User is not authorized, allowing access to guest pages.");
    return <>{children}</>;
}

export default GuestGuard