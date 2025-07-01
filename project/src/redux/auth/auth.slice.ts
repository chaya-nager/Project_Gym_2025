import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RoleType } from "../../types/user.types";

type User = {
  id: number;
  name: string;
  email: string;
  phone?: string;
  address?: string;
  role: RoleType;
};

type AuthState = {
  isAuthorized: boolean;
  isInitialized: boolean;
  user: User | null;
};

const initialState: AuthState = {
  isAuthorized: false,
  isInitialized: false,
  user: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setAuth: (state, action: PayloadAction<User>) => {
      state.user = action.payload;
      state.isAuthorized = true;
    },
    setInitialized: (state) => {
      state.isInitialized = true;
    },
    logout: (state) => {
      state.isAuthorized = false;
      state.user = null;
    },
  },
});

export const { setAuth, setInitialized, logout } = authSlice.actions;
export default authSlice.reducer;
