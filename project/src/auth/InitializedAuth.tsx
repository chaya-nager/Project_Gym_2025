import { useAppDispatch } from "../redux/store";
import { useEffect } from "react";
import {
  getSession,
  isValidToken,
  removeSession,
  setSession,
} from "./auth.utils";
import { setAuth, setInitialized } from "../redux/auth/auth.slice";
import { getUserByToken } from "../services/auth.service";

const InitializedAuth = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const initialized = async () => {
      const token = getSession();
      if (token && isValidToken(token)) {
        try {
          const user = await getUserByToken(token);
          setSession(token);
          dispatch(setAuth(user));
        } catch (error) {
          console.error("שגיאה בקבלת המשתמש לפי טוקן:", error);
          removeSession(false);
        }
      } else {
        removeSession(false);
      }

      dispatch(setInitialized());
    };

    initialized();
  }, [dispatch]);

  return null;
};

export default InitializedAuth;
