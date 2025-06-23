pipeline { 
    agent any

    environment {
        EC2_HOST = '13.233.160.125'
        EC2_USER = 'ubuntu'
        EC2_DEPLOY_DIR = '/home/ubuntu/todoapp'
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
                withCredentials([sshUserPrivateKey(credentialsId: env.CREDENTIALS_ID, keyFileVariable: 'KEY')]) {
                    bat """
                    powershell -Command "icacls '%KEY%' /inheritance:r"
                    powershell -Command "icacls '%KEY%' /grant:r \\"NT AUTHORITY\\\\SYSTEM:R\\""
                    powershell -Command "ssh -o StrictHostKeyChecking=no -o IdentitiesOnly=yes -i '%KEY%' ${EC2_USER}@${EC2_HOST} mkdir -p ${EC2_DEPLOY_DIR}"
                    powershell -Command "scp -o StrictHostKeyChecking=no -o IdentitiesOnly=yes -i '%KEY%' -r out\\* ${EC2_USER}@${EC2_HOST}:${EC2_DEPLOY_DIR}"
                    """
                }
            }
        }
    }
}
