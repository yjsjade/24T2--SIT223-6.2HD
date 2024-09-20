pipeline {
    agent {
        docker {
            image 'yjsjade/62hd-image'
        }
    }
    
    environment {
        SONARQUBE_TOKEN = credentials('sonar-token')
    }

    stages {
        stage('Checkout') {
            steps {
                sh 'git config --global --add safe.directory /var/jenkins_home/workspace/6.2HD'
                git branch: 'main', url: 'https://github.com/yjsjade/24T2--SIT223-6.2HD.git'
            }
        }
        
        stage('Build') {
            steps {
                sh 'dotnet publish Project/54HD.sln -c Release -o ./publish'
                sh 'docker build -t coin:test .'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test Projects.Test/Projects.Test.sln --logger:"console;verbosity=detailed"'
            }
        }

        stage('Code Quality Analysis') {
            steps {
                withSonarQubeEnv('SonarQube Server') {
                    sh 'dotnet sonarscanner begin /k:"62hd-project" /d:sonar.login=$SONARQUBE_TOKEN'
                    sh 'dotnet build'
                    sh 'dotnet sonarscanner end /d:sonar.login=$SONARQUBE_TOKEN'
                }
            }
        }

        stage('Deploy') {
            steps {
                sh 'docker-compose -f test.yml up -d'
            }
        }
        

        stage('Release') {
            steps {
                sh 'docker build -t coin:latest .'
                sh 'docker-compose -f prod.yml up -d'
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