# 🧠 Intelligent Knowledge Assistant (RAG + GenAI Backend)

## 📘 Overview  
This project implements a **Retrieval-Augmented Generation (RAG)** powered **Intelligent Knowledge Assistant**, enabling users to upload documents (PDF or Text or Docx), ask questions, and receive **context-aware AI-generated responses**.  

The system integrates **document retrieval**, **vector embeddings**, and **Generative AI (LLM)** to deliver accurate, context-based answers.  

---

## 🚀 Key Features  
✅ Upload and process **PDF or Text** documents  
✅ Automatic **text extraction**, **chunking**, and **embedding generation**  
✅ Store embeddings in a **vector database** - Pinecone  
✅ Retrieve **most relevant document chunks** using cosine similarity search  
✅ Generate **contextual AI responses** using **OpenAI API**  
✅ Maintain **chat history** and **conversation context**  
✅ Secure API endpoints with **JWT Authentication**  
✅ Scalable, modular backend architecture  
✅ Global Exception Handling for consistent API error responses  
✅ Custom Response Class for standardized API outputs  
✅ Dependency Injection (DI) for modular and testable service architecture 

---

### 🔹 Tech Stack
1. **Backend Service:** Built using `.NET Core Web API`  
2. **Vector Database:** Stores and retrieves vector embeddings using Pinecone   
3. **LLM Integration:** Generates responses using models like (Embedding Model -> text-embedding-3-small , Chat Model -> gpt-4o-mini)
4. **Relational DB :** SQL Server stores users, chat histories and exception logs.
5. **Authentication :** JWT Authentication 

---

## 🛠️ Setup Instructions for .Net

### 1️⃣ Clone the Repository
```bash
git clone https://gitlab.com/vishalyddv03/RAGChatBot.git
cd RAGChatBot
```
---

### 2️⃣ Obtain API Keys

#### OpenAI API Key
1. Sign up or log in at [OpenAI](https://platform.openai.com/).  
2. Navigate to **API Keys** in your account settings.  
3. Click **Create new secret key** and copy it.  
4. Keep it safe; you will need it for the `.env` or `appsettings.json` file.  

#### Pinecone API Key
1. Sign up or log in at [Pinecone](https://www.pinecone.io/).  
2. Navigate to **API Keys** in your account dashboard.  
3. Click **Create new API key** and copy it.  
4. Note your **environment name** (e.g., `us-east1-gcp`).  
5. Keep it safe; you will need it for the `.env` or `appsettings.json` file.

---

### 3️⃣ Configure appsettings.json file
```
"ConnectionStrings": {
  "dbcs": "Server=YourServerName;Database=YourDbName;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "YourSecretKey",
  "Issuer": "YourAudience",
  "Audience": "YourUsers"
},
"OpenAI": {
  "OpenAIApiKey": "YourAPIKey",
  "EmbeddingModel": "text-embedding-3-small",
  "ChatModel": "gpt-4o-mini"
},
"Pinecone": {
  "VectorApiKey": "YourAPIKey",
  "Host": "HostName",
  "Index": "IndexName",
  "Region": "YourRegion"
}
```

### 4️⃣ Install Dependencies 
```bash
dotnet restore
```
---

### 5️⃣ Database Setup with Migration Script

1. Open your Database Server Client like SSMS and run the [Migration Script](RAGChatBot.Infrastructure/SqlScripts/DbSetupScript.sql)
   
---

## 📚 References
- [OpenAI API Docs](https://platform.openai.com/docs)
- [Pinecone Docs](https://docs.pinecone.io)
- [OpenAI .NET SDK (NuGet)](https://www.nuget.org/packages/OpenAI/)
- [Pinecone.NET SDK (NuGet)](https://www.nuget.org/packages/Pinecone.NET/)

---

## 👨‍💻 Author
**Vishal Yadav**  
📧 [vishaljyadav576@gmail.com](mailto:vishaljyadav576@gmail.com)  



