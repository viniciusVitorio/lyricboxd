# 🎶 Lyricboxd

Lyricboxd is a social platform for sharing music tastes, where users can record their opinions about songs and albums as they listen to them.

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### 📋 Prerequisites

Make sure you have the following installed on your machine:

- 🐳 Docker
- 🐋 Docker Compose
- 🌐 .NET SDK (if necessary for additional development tasks)

### 📦 Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/your-username/lyricboxd.git
    cd lyricboxd
    ```

2. **Configure Docker:**

    Ensure Docker and Docker Compose are installed and running on your machine.

3. **Build and run the Docker containers:**

    ```bash
    docker-compose build
    docker-compose up
    ```

4. **Apply Entity Framework migrations:**

    Open a terminal in the web container and run the following command to apply the migrations:

    ```bash
    docker-compose exec web dotnet ef database update
    ```

5. **Access the application:**

    Your application will be available at `http://localhost:8000`.

## 📝 Notes

- Ensure the PostgreSQL database is running and accessible.
- Update connection settings as necessary in the `appsettings.json` file.

Enjoy sharing your musical tastes on Lyricboxd! 🎉🎧
