# Created by: Sangram Nandkhile on 5 Oct 2017 To fetch latest code for the current branch
branch=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
echo $branch
git pull origin $branch
echo Press Enter...
read