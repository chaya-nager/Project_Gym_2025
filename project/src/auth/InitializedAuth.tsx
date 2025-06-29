import { useAppDispatch } from "../redux/store"
import { useEffect } from "react"
import { getSession, isValidToken, removeSession, setSession } from "./auth.utils"
import { setAuth, setInitialized } from "../redux/auth/auth.slice"
import { getUserByToken } from "../services/auth.service"

const InitializedAuth = () => {
    const dispatch = useAppDispatch()

    useEffect(() => {
        const initialized = async () => {
            const token = getSession()
            if (token && isValidToken(token)) {
                // לבצע קריאת שרת המקבלת את פרטי המשתמש לפי token
                const user = await getUserByToken(token)
                setSession(token)
                dispatch(setAuth(user))
            }
            else {
                removeSession(false)
                dispatch(setInitialized())
            }
        }
        initialized()
    }, [])

    return null
}

export default InitializedAuth