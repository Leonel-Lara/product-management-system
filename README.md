# 🛒 Product Management System (Fullstack)

Sistema completo de gerenciamento de produtos com **API em .NET** e **Front-end em Vue 3 + Quasar**.

---

## 🚀 Tecnologias utilizadas

### 🔹 Backend

- .NET 10
- Entity Framework Core
- SQLite
- Swagger (OpenAPI)

### 🔹 Frontend

- Vue.js 3 (Composition API)
- Quasar Framework
- Axios
- Yarn

---

## 📦 Estrutura do projeto

/
├── backend/ → API .NET
├── frontend/ → Aplicação Vue + Quasar

---

# ⚙️ Como rodar o projeto

## 🔹 1. Clone o repositório

git clone https://github.com/Leonel-Lara/product-management-system.git
cd product-management-system

---

# 🧠 Backend (.NET)

## ▶️ Rodando a API

cd backend
dotnet restore
dotnet run

---

## 🌐 Acesso

Swagger:

http://localhost:5205/swagger

> A porta pode variar dependendo do ambiente

---

## 🗄️ Banco de dados

- Utiliza **SQLite**
- Criado automaticamente ao iniciar
- Possui **seed com 14 produtos**

✔ Não precisa configurar nada manualmente

---

## ⚠️ Regras de negócio

- Estoque não pode ser negativo
- SKU deve ser único
- Eletrônicos possuem preço mínimo de R$ 50

---

# 💻 Frontend (Vue + Quasar)

## ▶️ Rodando o front

cd frontend
yarn
yarn dev

---

## 🌐 Acesso

http://localhost:9000

---

## 🔌 Integração com API

O front consome a API via Axios:

baseURL: 'http://localhost:5205/api'

---

## ⚠️ Problema comum (CORS)

Se houver erro de conexão:

👉 Verifique se o backend está rodando
👉 Confirme a porta da API

---

## 🧠 Funcionalidades

- Listagem de produtos (com paginação)
- Criação de produto
- Edição de produto
- Exclusão de produto
- Feedback visual de erros (Dialog do Quasar)

---

## 🧪 Testando o sistema

1. Inicie o backend
2. Inicie o frontend
3. Acesse o front
4. Interaja com os produtos

---

# 🔥 Observações importantes

- O banco é criado automaticamente
- O seed inicial garante dados para teste
- Projeto pronto para ambiente real com pequenos ajustes

---

# 👨‍💻 Autor

Desenvolvido por **Leonel Lara**
