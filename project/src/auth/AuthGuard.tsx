import { ReactNode } from "react";
import { Navigate } from "react-router";
import { useAppSelector } from "../redux/store";
import { selectAuth } from "../redux/auth/auth.selector";
import { Paths } from "../routes/paths";
import { RoleType } from "../types/user.types";
import { UserType } from "../types/user.types"; 
type Props = {
  children: ReactNode;
  roles?: RoleType[];
};

const AuthGuard = ({ children, roles }: Props) => {
  const { isAuthorized, isInitialized, user } = useAppSelector(selectAuth) as {
    isAuthorized: boolean;
    isInitialized: boolean;
    user: UserType | null;
  };
  if (!isInitialized) return <h1>Loading...</h1>;

  if (!isAuthorized) {
    return <Navigate to={`/${Paths.auth}/${Paths.login}`} replace />;
  }
  const userRole = user?.role as RoleType | undefined;

  if (roles?.length && (!userRole || !roles.includes(userRole))) {
    console.warn("🚫 Unauthorized: role is", userRole);
    return <h1>אין הרשאה</h1>;
  }

  return <>{children}</>;
};

export default AuthGuard;
