def externalMethod
def details = ""
node ('master'){
	details = "${env.JOB_NAME}".split("/")
	    if(details[0] == 'BikewaleProductionPipeline'){
			externalMethod = load("D:\\JenkinsUtilities\\bikewale\\Production\\Production.groovy")
	    }
	else if(details[0]=='BikewaleStagingPipeline'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\Staging\\Staging.groovy")
	}
	else if(details[0]=='BikewaleFeatureTestingPipeline'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\FeatureTesting\\FeatureTesting.groovy")
	}


     //Call the method we defined in externalMethod.
}
if(details[0] == 'BikewaleProductionPipeline'){
	externalMethod.Production()
}
else if(details[0]=='BikewaleStagingPipeline'){
	externalMethod.Staging()
}
else if(details[0]=='BikewaleFeatureTestingPipeline'){
	externalMethod.Testing()
}