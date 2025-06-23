pipeline { 
    agent any

    environment {
        EC2_HOST = '13.233.160.125'
        EC2_USER = 'ubuntu'
        EC2_DEPLOY_DIR = '/home/ubuntu/todoapp'
        CREDENTIALS_ID = 'ec2-ssh'            // Jenkins SSH key for EC2
        GIT_CREDENTIALS_ID = 'github-ssh-key' // Jenkins SSH key for GitHub
    }

    stages {
        stage('Clone') {
            steps {
                checkout([$class: 'GitSCM',
                    branches: [[name: '*/main']],
                    userRemoteConfigs: [[
                        url: 'git@github.com:Nikunj-19/my-TO-DO-APP.git',
                        credentialsId: env.GIT_CREDENTIALS_ID
                    ]]
                ])
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
                    powershell -Command "icacls '%KEY%' /remove:g BUILTIN\\Users"
                    powershell -Command "icacls '%KEY%' /grant:r `"$env:USERNAME:R`""
                    powershell -Command "ssh -i '%KEY%' -o StrictHostKeyChecking=no -o IdentitiesOnly=yes ${EC2_USER}@${EC2_HOST} mkdir -p ${EC2_DEPLOY_DIR}"
                    powershell -Command "scp -i '%KEY%' -o StrictHostKeyChecking=no -o IdentitiesOnly=yes -r out\\* ${EC2_USER}@${EC2_HOST}:${EC2_DEPLOY_DIR}"
                    """
                }
            }
        }
    }
}
