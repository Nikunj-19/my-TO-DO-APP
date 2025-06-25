pipeline { 
    agent any

    environment {
        EC2_HOST = credentials('ec2-host') // Store in Jenkins credentials
        EC2_USER = 'ubuntu'
        EC2_DEPLOY_DIR = '/home/ubuntu/todoapp'
        CREDENTIALS_ID = 'ec2-ssh'
        GIT_CREDENTIALS_ID = 'github-ssh-key'
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

        stage('Test') {
            steps {
                bat 'dotnet test --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish --configuration Release --output out'
            }
        }

        stage('Test SSH') {
            steps {
                sshagent (credentials: [env.CREDENTIALS_ID]) {
                    bat '''
                    echo Testing SSH connection to EC2...
                    ssh -o StrictHostKeyChecking=yes -o IdentitiesOnly=yes %EC2_USER%@%EC2_HOST% echo SSH Connected || exit 1
                    '''
                }
            }
        }

        stage('Deploy to EC2') {
            steps {
                sshagent (credentials: [env.CREDENTIALS_ID]) {
                    bat '''
                    ssh -o StrictHostKeyChecking=yes -o IdentitiesOnly=yes %EC2_USER%@%EC2_HOST% "mkdir -p %EC2_DEPLOY_DIR% && rm -rf %EC2_DEPLOY_DIR%/*"
                    scp -o StrictHostKeyChecking=yes -o IdentitiesOnly=yes -r out/* %EC2_USER%@%EC2_HOST%:%EC2_DEPLOY_DIR%
                    ssh -o StrictHostKeyChecking=yes -o IdentitiesOnly=yes %EC2_USER%@%EC2_HOST% "cd %EC2_DEPLOY_DIR% && sudo systemctl restart myapp"
                    '''
                }
            }
        }
    }

    post {
        failure {
            echo 'Pipeline failed! Notifying team...'
            // Add notification (e.g., Slack, email)
        }
        success {
            echo 'Deployment successful!'
        }
    }
}
