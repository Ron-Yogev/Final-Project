# Final-Project

Link:
<https://ronyogev.itch.io/mix-it>

## prerequisites - Server Configuration
  

### Step 1: Xammp Download
        
 Download xammp from here: 
 
 <https://www.apachefriends.org/download.html>       

### Step 2: Xammp Installation 
 
 Install xammp in the default folder and check this components from the list:
 
 ![](configuration%20images/xammp%20installation.png)
         
### Step 3: Run SQL+APACHE server   

  Run Xammp and click start on SQL+APACHE actions: 
  
  ![](configuration%20images/xammp%20start.png)
  
  SQL and APACHE modules should be green after you started them.
  
* Note that SQL server need to be run only from XAMMP and not from another third party (proccess)


### Step 4: Import DB configuration

  Enter to this URL: 
<http://localhost/phpmyadmin/>

In your left, click 'New' for create new database:

![](configuration%20images/new%20db.png)

After that, name the database 'mixit' and click 'Go', and you will see database named 'mixit' in your left.

Select 'mixit' database on the left pane, and click on the Import tab in the top center pane.

![](configuration%20images/import%20db.png)

Download 'mixit.zip' file from this Project's main folder, and extract 'mixit.sql' file.

Under the File to import section, click Browse and locate the mixit.sql file:

![](configuration%20images/import%20browse.png)

and click 'Go'


### Step 5: Import Server files 

Go to 'C:\xampp\htdocs' folder, and create new folder named 'UnityBackend'.

Inside 'UnityBackend' folder you created, extract from 'mixit.zip' file all PHP files:


