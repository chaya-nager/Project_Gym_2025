import { createBrowserRouter, RouterProvider, Navigate } from "react-router";
import HomePage from "../pages/HomePage";
import { ProductsPage } from "../pages/ProductsPage";
import { AuthPage } from "../pages/AuthPage";
import { LoginPage } from "../pages/LoginPage";
import { SignUpPage } from "../pages/SignUpPage";
import { Layout } from "../layouts/Layout";
import { Paths } from "./paths";
import AuthGuard from "../auth/AuthGuard";
import GuestGuard from "../auth/guestGuard";
import { RoleType } from "../types/user.types";
import { AdminPage } from "../pages/AdminPage";
import WorkoutPlanPage from "../pages/WorkoutPlanPage"; // <-- ודא שקובץ זה קיים

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
          path: "/workout-plan/:id", // <-- נתיב חדש לעמוד תכנית אימון
          element: <WorkoutPlanPage />,
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
      element: <Navigate to="/home" />,
    },
  ]);

  return <RouterProvider router={router} />;
};

export default Router;
