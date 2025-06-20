pipeline {
    agent any

    environment {
        EC2_HOST = 'your-ec2-ip'
        EC2_USER = 'ec2-user'
        EC2_DEPLOY_DIR = '/home/ec2-user/todoapp'
        CREDENTIALS_ID = 'ec2-ssh'
    }

    stages {
        stage('Clone') {
            steps {
                git url: 'https://github.com/Nikunj-19/my-TO-DO-APP.git', branch: 'main'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish --configuration Release --output out'
            }
        }

        stage('Deploy to EC2') {
            steps {
                sshagent (credentials: [env.CREDENTIALS_ID]) {
                    bat """
                    powershell -Command "ssh -o StrictHostKeyChecking=no ${EC2_USER}@${EC2_HOST} mkdir -p ${EC2_DEPLOY_DIR}"
                    powershell -Command "scp -o StrictHostKeyChecking=no -r out\\* ${EC2_USER}@${EC2_HOST}:${EC2_DEPLOY_DIR}"
                    """
                }
            }
        }
    }
}
