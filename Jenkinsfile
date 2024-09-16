pipeline {
    agent any

    environment {
        DOTNET_ROOT = 'C:\\Program Files\\dotnet'
    }

    stages {
        stage('Checkout and restore dependencies') {
            steps {
                git branch: 'main', url: 'https://github.com/yjsjade/24T2--SIT223-6.2HD.git'
            }
        }

        stage('Build') {
            steps {
                // Build the .NET project
                bat 'dotnet build 54HD.sln --configuration Release'
            }
        }

        stage('Test') {
            steps {
                // Run tests using dotnet test
                bat 'dotnet test Project.Tests/54HD.csproj --no-build'
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
                    withSonarQubeEnv('SonarQube') {
                        bat 'dotnet sonarscanner begin /k:"your-sonarqube-key" /d:sonar.login=${SONARQUBE_TOKEN}'
                        bat 'dotnet build'
                        bat 'dotnet sonarscanner end /d:sonar.login=${SONARQUBE_TOKEN}'
                    }
                }
            }
        }

        stage('Deploy to Test Environment') {
            steps {
                bat 'dotnet publish YourProject.sln -c Release -o ./publish'
                bat 'docker build -t coin:test ./publish'
                bat 'docker run -d -p 8081:80 coin:test'
            }
        }
        

        stage('Release to Production') {
            steps {
                bat 'docker build -t coin:latest ./publish'
                bat 'docker run -d -p 81:80 coin:latest'
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