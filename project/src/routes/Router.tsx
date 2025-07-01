import { createBrowserRouter, RouterProvider, Navigate } from "react-router";
import HomePage from "../pages/HomePage";
import { ProductsPage } from "../pages/ProductsPage";
import { AuthPage } from "../pages/AuthPage";
import LoginPage  from "../pages/LoginPage";
import { SignUpPage } from "../pages/SignUpPage";
import { Layout } from "../layouts/Layout";
import { Paths } from "./paths";
import AuthGuard from "../auth/AuthGuard";
import GuestGuard from "../auth/guestGuard";
import { RoleType } from "../types/user.types";
import { AdminPage } from "../pages/AdminPage";
import WorkoutPlanPage from "../pages/WorkoutPlanPage";
import UploadVideoPage from "../pages/UploadVideoPage"; 
import WorkoutPlanForm from "../pages/WorkoutPlanForm"; // 👈 ייבוא קיים

const Router = () => {
  const router = createBrowserRouter([
    {
      element: <Layout />,
      children: [
        {
          path: Paths.home,
          element: <HomePage />,
        },
        {
          path: Paths.products,
          element: (
            <AuthGuard roles={[RoleType.User]}>
              <ProductsPage />
            </AuthGuard>
          ),
        },
        {
          path: Paths.admin,
          element: (
            <AuthGuard roles={[RoleType.Admin]}>
              <AdminPage />
            </AuthGuard>
          ),
        },
        {
          path: "/workout-plan/:id",
          element: <WorkoutPlanPage />,
        },
        {
          path: "/upload-video", 
          element: (
            <AuthGuard roles={[RoleType.Trainer]}>
              <UploadVideoPage />
            </AuthGuard>
          ),
        },
        {
          path: "/choose-plan", // ✅ הנתיב החדש שהוספנו
          element: (
            <AuthGuard roles={[RoleType.User]}>
              <WorkoutPlanForm />
            </AuthGuard>
          ),
        },
      ],
    },
    {
      path: "*",
      element: <h1>404 Not Found</h1>,
    },
    {
      path: "auth",
      element: (
        <GuestGuard>
          <AuthPage />
        </GuestGuard>
      ),
      children: [
        {
          path: Paths.login,
          element: <LoginPage />,
        },
        {
          path: "sign-up",
          element: <SignUpPage />,
        },
      ],
    },
    {
      index: true,
      element: (
        <AuthGuard roles={[RoleType.User]}>
          <Navigate to="/choose-plan" />
        </AuthGuard>
      ),
    }
  ]);

  return <RouterProvider router={router} />;
};

export default Router;
