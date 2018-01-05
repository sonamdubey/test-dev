def externalMethod
def details = ""
node ('master'){
	details = "${env.JOB_NAME}".split("/")
	if(details[0] == 'BikeWaleProductionPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\Production\\Production.groovy")
	}
	else if(details[0]=='BikeWaleStagingPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\Staging\\Staging.groovy")
	}
	else if(details[0]=='BikeWaleFeatureTestingPipeline'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\FeatureTesting\\FeatureTesting.groovy")
	}
	else if(details[0]=='BikeWaleProductionTestingPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\ProductionTesting\\ProductionTesting.groovy")
	}
	else if(details[0]=='BikewaleSonarQubeAnalysis'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\SonarQube\\SonarQube.groovy")	
	}
     //Call the method we defined in externalMethod.
}
if(details[0] == 'BikeWaleProductionPipeline'){
	externalMethod.Production()
}
else if(details[0]=='BikeWaleStagingPipeline'){
	externalMethod.Staging()
}
else if(details[0]=='BikeWaleFeatureTestingPipeline'){
	externalMethod.Testing()
}
else if(details[0]=='BikeWaleProductionTestingPipeline'){
	externalMethod.ProductionTesting()
}
else if(details[0]=='BikewaleSonarQubeAnalysis'){
	externalMethod.RunAnalysis() 	
}
