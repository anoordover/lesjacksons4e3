https://youtu.be/DgVjEo3OGBI?t=39044
GRPC to k8s


# build

docker build -t anoordover/platformservice .  
docker build -t anoordover/commandservice .  

# push
docker push anoordover/platformservice  
docker push anoordover/commandservice  

# kubernetes

microk8s start  
microk8s kubectl get deployments  
microk8s kubectl get pods  
microk8s kubectl get services  
microk8s kubectl rollout restart deployment commands-depl  
microk8s kubectl rollout restart deployment platforms-depl  
