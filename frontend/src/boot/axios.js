import axios from 'axios'
import { Dialog } from 'quasar'

// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)
const api = axios.create({ baseURL: 'http://localhost:5205/api' })

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const status = error?.response?.status
    const data = error?.response?.data

    const message =
      typeof data === 'string'
        ? data
        : data?.message || 'Algo deu errado. Tente novamente mais tarde.'

    if (status === 500) {
      showErrorDialog('Erro interno [500]', 'Algo deu errado. Tente novamente mais tarde.')
    } else if (status) {
      showErrorDialog('Algo deu errado', message)
    } else {
      showErrorDialog('Falha de conexão', 'Não foi possível conectar ao servidor.')
    }

    return Promise.reject(error)
  },
)

function showErrorDialog(title, message) {
  Dialog.create({
    title: `<b class="text-red">${title}</b>`,
    message: message,
    persistent: false,
    html: true,
    ok: { color: 'red-9' },
  })
}

export { api }
