import { UserType } from "../types/user.types"
import axios from "./axios"

const url = 'LogIn'

export const login = async (formData: FormData) => {
    const response = await axios.post<string>('/LogIn/login', formData);
    return response.data;
  };

export const getUserByToken = async (token: string) => {
    axios.defaults.headers.common.Authorization = `Bearer ${token}`
    const response = await axios.get<UserType>(url + '/GetUserByToken')
    return response.data
}

export const signUp = async () => {

}