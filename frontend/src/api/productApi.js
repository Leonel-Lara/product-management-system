import { api } from 'boot/axios'

const getList = async (params) => {
  const response = await api.get('/products', { params })
  return response
}

const getDetails = async (objId) => {
  const response = await api.get(`/products/${objId}`)
  return response
}

const postItem = async (obj) => {
  const response = await api.post('/products', obj)
  return response
}

const putItem = async (obj) => {
  const response = await api.put(`/products/${obj.id}`, obj)
  return response
}

const deleteItem = async (objId) => {
  const response = await api.delete(`/products/${objId}`)
  return response
}

export default {
  getList,
  getDetails,
  postItem,
  putItem,
  deleteItem,
}
