import { useState, useEffect } from "react"
import { getCategories } from "../services/category.service"
import { CategoryType } from "../types/category.types"

const Category = () => {
    const [category, setCategory] = useState<CategoryType[]>([])


    useEffect(() => {
        const getData = async () => {
            const data = await getCategories()
            setCategory(data)
        }
        getData()
    }, [])

    return <>{category.map(category => (
        <div key={category.id}>{category.name}</div>
    ))}</>
}

export default Category