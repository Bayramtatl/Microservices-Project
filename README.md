[TR]

# 🚀 Microservices Architecture: Advanced E-Commerce Backend

Bu proje, modern yazılım prensipleri kullanılarak inşa edilmiş, ölçeklenebilir ve dayanıklı bir **Mikroservis** mimarisi örneğidir. Proje; güvenlikten izlenebilirliğe, otomasyondan konteynerizasyona kadar tam donanımlı bir DevOps ekosistemi üzerine kurulmuştur.

---

## 🛠️ Kullanılan Teknolojiler ve Araçlar

### **Core Frameworks & Languages**
* **.NET 8.0:** Yüksek performanslı API servisleri için temel framework.
* **C#:** Tip güvenli ve nesne yönelimli uygulama geliştirme.

### **Architecture & Communication**
* **Ocelot API Gateway:** Tüm isteklere tek bir giriş noktası sağlar, rotalama ve yetkilendirme yapar.
* **IdentityServer4 / JWT:** Servisler arası ve kullanıcı bazlı güvenlik katmanı.
* **Microservices:** Her iş mantığı (Sepet, Katalog vb.) kendi izole servisinde çalışır.

### **DevOps & Infrastructure**
* **Docker & Docker Compose:** Servislerin her ortamda aynı şekilde çalışması için konteynerizasyon.
* **Kubernetes (K8s):** Servis orkestrasyonu ve yük dengeleme.
* **GitHub Actions (CI/CD):** Otomatik derleme, test ve Docker Hub'a dağıtım süreçleri.

### **Observability & Quality**
* **ELK Stack (Elasticsearch, Logstash, Kibana):** Merkezi loglama ve gerçek zamanlı analiz.
* **Serilog:** Yapılandırılmış (structured) log üretimi.
* **SonarQube:** Statik kod analizi ve kod kalitesi ölçümü.

---

## 🏗️ Sistem Mimarisi ve Akış (Flow)

Proje, istemciden gelen bir isteğin en güvenli ve izlenebilir yoldan hedefe ulaşmasını sağlar:

1.  **Entry Point (Ocelot):** İstemci, servislere doğrudan değil, API Gateway üzerinden erişir. Gateway, isteğin rotasını belirler.
2.  **Authentication (Identity Service):** Korumalı bir rotaya erişilmek istendiğinde, sistem kullanıcıdan geçerli bir Bearer Token bekler. Gateway, token doğruluğunu kontrol eder.
3.  **Service Execution:** Yetkilendirilmiş istek, ilgili mikroservise (örn: `Basket.API`) yönlendirilir.
4.  **Logging (ELK):** İstek sırasında oluşan tüm olaylar Serilog aracılığıyla yakalanır, Logstash üzerinden Elasticsearch'e iletilir ve Kibana'da görselleştirilir.



---

## 📦 Kurulum ve Çalıştırma

### **Ön Gereksinimler**
* Docker Desktop
* .NET 8 SDK
* Git

### **Adımlar**
1.  Projeyi klonlayın:
    ```bash
    git clone [https://github.com/kullaniciadin/Microservices-Project.git](https://github.com/kullaniciadin/Microservices-Project.git)
    ```
2.  Docker Compose ile tüm ekosistemi ayağa kaldırın:
    ```bash
    docker-compose up -d
    ```
3.  Servislere erişin:
    * **API Gateway:** `http://localhost:5000`
    * **Kibana:** `http://localhost:5601`
    * **SonarQube:** `http://localhost:9000`

---

## 🤖 CI/CD Süreçleri (GitHub Actions)

Proje, tam otomatize edilmiş bir CI/CD hattına sahiptir. `.github/workflows` dizini altında tanımlanan hatlar şu adımları izler:

* **Trigger:** `Identity.API` gibi spesifik klasörlerde kod değişikliği olduğunda tetiklenir.
* **Build & Test:** Kodun derlendiğinden ve unit testlerden geçtiğinden emin olunur.
* **SonarQube Scan:** Kod kalitesi taranır (Technical Debt, Code Smells).
* **Docker Push:** Başarılı olan her yapı, otomatik olarak Docker Hub üzerindeki ilgili repository'e `latest` tag'i ile itilir.

---

## 📊 İzlenebilirlik (Monitoring & Logging)

Sistemdeki "kara kutu" sorununu çözmek için şu mekanizmalar kullanılır:
* **Merkezi Loglama:** Hata veya bilgi logları tek bir merkezden (Kibana) takip edilir. Örneğin; `Request for authenticated route was unauthenticated!` gibi güvenlik logları anlık izlenebilir.
* **Kod Analizi:** SonarQube ile kodun sürdürülebilirliği kontrol altında tutulur.

---

## 🗺️ Yol Haritası (Roadmap)
- [x] API Gateway & Identity Entegrasyonu
- [x] ELK Stack ile Merkezi Loglama
- [x] Docker & Kubernetes Yapılandırması
- [x] GitHub Actions CI/CD Pipeline
- [x] SonarQube Analiz Katmanı
- [x] RabbitMQ ile Asenkron İletişim (Sırada!)
- [x] Redis Distributed Caching Entegrasyonu

[EN]

# 🚀 Microservices Architecture: Advanced E-Commerce Backend

This project is a robust example of a scalable and resilient **Microservices** architecture built with modern software engineering principles. It features a full-blown DevOps ecosystem covering everything from security to observability and automated delivery.

---

## 🛠️ Tech Stack & Tools

### **Core Frameworks & Languages**
* **.NET 8.0:** High-performance framework for building the core API services.
* **C#:** Type-safe and object-oriented programming for business logic.

### **Architecture & Communication**
* **Ocelot API Gateway:** Serves as a single entry point for all clients, handling routing and request aggregation.
* **IdentityServer4 / JWT:** Secure layer for cross-service and user-based authentication.
* **Microservices:** Decoupled services (e.g., Basket, Catalog) operating in their own isolated environments.

### **DevOps & Infrastructure**
* **Docker & Docker Compose:** Containerization for consistent environments across development and production.
* **Kubernetes (K8s):** Orchestration for managing service scaling and load balancing.
* **GitHub Actions (CI/CD):** Automated build, test, and deployment pipelines to Docker Hub.

### **Observability & Quality**
* **ELK Stack (Elasticsearch, Logstash, Kibana):** Centralized logging and real-time data visualization.
* **Serilog:** Structured logging for better diagnostic capabilities.
* **SonarQube:** Static code analysis to maintain high code quality and security standards.

---

## 🏗️ System Architecture & Workflow

The system ensures that every client request follows a secure and traceable path:

1.  **Entry Point (Ocelot):** Clients interact solely with the API Gateway. The Gateway manages request routing to the correct service.
2.  **Authentication (Identity Service):** For protected routes, the system validates the Bearer Token. The Gateway ensures only authorized requests pass through.
3.  **Service Execution:** The authorized request is forwarded to the target microservice (e.g., `Basket.API`).
4.  **Logging (ELK):** All events are captured via Serilog, processed through Logstash, stored in Elasticsearch, and visualized in Kibana for monitoring.



---

## 📦 Installation & Setup

### **Prerequisites**
* Docker Desktop
* .NET 8 SDK
* Git

### **Steps**
1.  Clone the repository:
    ```bash
    git clone [https://github.com/yourusername/Microservices-Project.git](https://github.com/yourusername/Microservices-Project.git)
    ```
2.  Spin up the entire ecosystem using Docker Compose:
    ```bash
    docker-compose up -d
    ```
3.  Access the services:
    * **API Gateway:** `http://localhost:5000`
    * **Kibana:** `http://localhost:5601`
    * **SonarQube:** `http://localhost:9000`

---

## 🤖 CI/CD Pipelines (GitHub Actions)

This project utilizes a fully automated CI/CD pipeline. Defined under `.github/workflows`, the pipeline follows these steps:

* **Trigger:** Activated on code pushes to specific directories like `Identity.API`.
* **Build & Test:** Ensures the code compiles successfully and passes all unit tests.
* **SonarQube Scan:** Scans for technical debt, code smells, and security vulnerabilities.
* **Docker Push:** Upon success, a Docker image is automatically built and pushed to Docker Hub with the `latest` tag.

---

## 📊 Observability (Monitoring & Logging)

To eliminate "black box" issues in the distributed system, the following are implemented:
* **Centralized Logging:** All logs are searchable in Kibana. For example, security alerts like `Unauthenticated request attempt` can be monitored in real-time.
* **Code Quality:** SonarQube ensures that the codebase remains maintainable and secure over time.

---

## 🗺️ Roadmap
- [x] API Gateway & Identity Integration
- [x] Centralized Logging with ELK Stack
- [x] Docker & Kubernetes Configuration
- [x] GitHub Actions CI/CD Pipeline
- [x] SonarQube Analysis Integration
- [ ] RabbitMQ for Asynchronous Messaging (Next Step!)
- [ ] Redis Distributed Caching Implementation
