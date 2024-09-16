pipeline {
    agent any

    environment {
        docker { image 'docker:latest' }
    }

    stages {
        stage('Checkout and restore dependencies') {
            steps {
                git branch: 'main', url: 'https://github.com/yjsjade/24T2--SIT223-6.2HD.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    sh 'docker build -t my-dotnet-app .'
                }
            }
        }

        stage('Run Docker Container') {
            steps {
                script {
                    sh 'docker run -d -p 8080:80 my-dotnet-app'
                }
            }
        }

        stage('Build') {
            steps {
                // Assuming dotnet is available in the Docker container
                sh 'dotnet build Projects/54HD.sln --configuration Release'
            }
        }

        stage('Test') {
            steps {
                // Run tests using dotnet test
                sh 'dotnet test Projects/54HD.csproj --no-build'
            }
            post {
                always {
                    // Publish test results using JUnit
                    junit '**/TestResults/*.xml'
                }
            }
        }

        stage('Code Quality Analysis') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    // SonarQube scan for code quality
                    sh 'dotnet sonarscanner begin /k:"your-sonarqube-key" /d:sonar.login=${SONARQUBE_TOKEN}'
                    sh 'dotnet build'
                    sh 'dotnet sonarscanner end /d:sonar.login=${SONARQUBE_TOKEN}'
                }
            }
        }

        stage('Deploy to Test Environment') {
            steps {
                sh 'dotnet publish Projects/54HD.sln -c Release -o ./publish'
                sh 'docker build -t coin:test ./publish'
                sh 'docker run -d -p 8081:80 coin:test'
            }
        }
        
        stage('Release to Production') {
            steps {
                sh 'docker build -t coin:latest ./publish'
                sh 'docker run -d -p 81:80 coin:latest'
            }
        }

        stage('Monitoring and Alerting') {
            steps {
                script {
                    echo 'Monitoring production using Datadog or New Relic...'
                }
            }
        }
    }
}
