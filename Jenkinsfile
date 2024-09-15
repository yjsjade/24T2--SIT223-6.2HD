pipeline {
    agent any

    environment {
        DOTNET_ROOT = '"C:\Program Files\dotnet"'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/yjsjade/24T2--SIT223-6.2HD.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                // Build the .NET project
                sh 'dotnet build --configuration Release'
            }
            post {
                success {
                    // Archive build artifacts (e.g., DLLs)
                    archiveArtifacts artifacts: '**/bin/Release/**/*.dll', fingerprint: true
                }
            }
        }

        stage('Test') {
            steps {
                // Run tests using dotnet test
                sh 'dotnet test --no-build'
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
                    sh 'dotnet sonarscanner begin /k:"your-sonarqube-key" /d:sonar.login=$SONARQUBE_TOKEN'
                    sh 'dotnet build'
                    sh 'dotnet sonarscanner end /d:sonar.login=$SONARQUBE_TOKEN'
                }
            }
        }

        stage('Package') {
            steps {
                // Create a release package of the application
                sh 'dotnet publish -c Release -o ./publish'
            }
        }

        stage('Deploy to Test Environment') {
            steps {
                // Example Docker deployment step
                sh 'docker build -t your-docker-repo/yourapp:${env.BUILD_ID} .'
                sh 'docker run -d -p 8080:80 your-docker-repo/yourapp:${env.BUILD_ID}'
            }
        }

        stage('Release to Production') {
            steps {
                echo 'Deploying to production...'
                // Add production deployment steps (e.g., IIS, Docker, Kubernetes, etc.)
            }
        }
    }
}
