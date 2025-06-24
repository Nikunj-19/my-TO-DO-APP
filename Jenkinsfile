pipeline { 
    agent any

    environment {
        EC2_HOST = '13.201.18.192'
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

        stage('Test SSH Agent') {
            steps {
                sshagent (credentials: ['ec2-ssh']) {
                    bat 'ssh -o StrictHostKeyChecking=no -T %EC2_USER%@%EC2_HOST% || echo SSH Failed'
                }
            }
        }

        stage('Deploy to EC2') {
            steps {
               sshagent (credentials: ['ec2-ssh'])  {
                    bat '''
                    ssh -o StrictHostKeyChecking=no -o IdentitiesOnly=yes %EC2_USER%@%EC2_HOST% "mkdir -p %EC2_DEPLOY_DIR%"
                    scp -o StrictHostKeyChecking=no -o IdentitiesOnly=yes -r out\\* %EC2_USER%@%EC2_HOST%:%EC2_DEPLOY_DIR%
                    '''
                }
            }
        }
    }
}
