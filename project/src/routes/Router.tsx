import { createBrowserRouter, RouterProvider, Navigate } from "react-router";
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
import VideoPlayer from "../pages/VideoPlayer";

const Router = () => {
  const router = createBrowserRouter([
    {
      path: "/", // כל מה שקשור ל־Layout
      element: <Layout />,
      children: [
        {
          index: true, // עמוד הבית ברירת מחדל
          element: <HomePage />,
        },
        {
          path: Paths.uploadVideo, // "upload-video"
          element: (
            <AuthGuard roles={[RoleType.Trainer]}>
              <UploadVideoPage />
            </AuthGuard>
          ),
        },
        {
          path: Paths.choosePlan, // "choose-plan"
          element: (
            <AuthGuard roles={[RoleType.User]}>
              <WorkoutPlanForm />
            </AuthGuard>
          ),
        },
        {
          path: Paths.videoPlayer, // "video-player"
          element: <VideoPlayer />,
        },
      ],
    },
    {
      path: Paths.auth, // "auth"
      element: (
        <GuestGuard>
          <AuthPage />
        </GuestGuard>
      ),
      children: [
        {
          path: Paths.login, // "login"
          element: <LoginPage />,
        },
        {
          path: "sign-up", // sign-up עדיין לא מוגדר לך ב־Paths, אפשר להוסיף אם צריך
          element: <SignUpPage />,
        },
      ],
    },
    {
      path: "*",
      element: <h1>404 - הדף לא נמצא</h1>,
    },
  ]);

  return <RouterProvider router={router} />;
};

export default Router;
