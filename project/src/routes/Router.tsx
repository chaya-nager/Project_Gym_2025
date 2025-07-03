import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "../pages/HomePage";
import { AuthPage } from "../pages/AuthPage";
import LoginPage from "../pages/LoginPage";
import { SignUpPage } from "../pages/SignUpPage";
import { Layout } from "../layouts/Layout";
import { Paths } from "./paths";
import AuthGuard from "../auth/AuthGuard";
import GuestGuard from "../auth/guestGuard";
import { RoleType } from "../types/user.types";
import UploadVideoPage from "../pages/UploadVideoPage";
import WorkoutPlanForm from "../pages/WorkoutPlanForm";
import LogoutPage from "../pages/LogoutPage";
import MyTrainerVideosPage from "../pages/MyTrainerVideosPage";
import EditVideoPage from "../pages/EditVideoPage";

const router = createBrowserRouter([
  {
    path: Paths.home,
    element: <Layout />,
    children: [
      { index: true, element: <HomePage /> },

      {
        path: Paths.uploadVideo,
        element: (
          <AuthGuard roles={[RoleType.Trainer]}>
            <UploadVideoPage />
          </AuthGuard>
        ),
      },
      {
        path: Paths.choosePlan,
        element: (
          <AuthGuard roles={[RoleType.User]}>
            <WorkoutPlanForm />
          </AuthGuard>
        ),
      },
      {
        path: Paths.myTrainerVideos,
        element: (
          <AuthGuard roles={[RoleType.Trainer]}>
            <MyTrainerVideosPage />
          </AuthGuard>
        ),
      },
      {
        path: `${Paths.editVideo}/:id`,
        element: (
          <AuthGuard roles={[RoleType.Trainer]}>
            <EditVideoPage />
          </AuthGuard>
        ),
      },
    ],
  },

  {
    path: Paths.auth,
    element: (
      <GuestGuard>
        <AuthPage />
      </GuestGuard>
    ),
    children: [
      { path: "login", element: <LoginPage /> },
      { path: "sign-up", element: <SignUpPage /> },
    ],
  },

  {
    path: Paths.logout,
    element: <LogoutPage />,
  },

  {
    path: "*",
    element: <h1 style={{ textAlign: "center", marginTop: "5rem" }}>404 - הדף לא נמצא</h1>,
  },
]);

const Router = () => <RouterProvider router={router} />;
export default Router;
