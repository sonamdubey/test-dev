def externalMethod
def details = ""
node ('master'){
	details = "${env.JOB_NAME}".split("/")
	if(details[0] == 'BikewaleProductionPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\Production\\Production.groovy")
	}
	else if(details[0]=='BikewaleStagingPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\Staging\\Staging.groovy")
	}
	else if(details[0]=='BikewaleFeatureTestingPipeline'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\FeatureTesting\\FeatureTesting.groovy")
	}
	else if(details[0]=='BikewaleProductionTestingPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\ProductionTesting\\ProductionTesting.groovy")
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