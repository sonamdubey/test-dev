def externalMethod
def details = ""
node ('master'){
	details = "${env.JOB_NAME}".split("/")
	if(details[0]=='BikeWaleStagingPipeline-New'){
		externalMethod = load("${BikewaleCI}" + "\\Staging-new\\Staging.groovy")
		externalMethod.Staging()
	}
	else if(details[0]=='NewBikeWaleProductionPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\production-new\\Production.groovy")
		externalMethod.Production()
	}
	else if(details[0]=='BikeWaleFeatureTestingPipeline'){
		externalMethod = load("D:\\JenkinsUtilities\\bikewale\\FeatureTesting\\FeatureTesting.groovy")
		externalMethod.Testing()
	}
	else if(details[0]=='BikeWaleProductionTestingPipeline'){
		externalMethod = load("${BikewaleCI}" + "\\ProductionTesting\\ProductionTesting.groovy")
		externalMethod.ProductionTesting()
	}
	else if(details[0]=='BikewaleSonarQubeAnalysis-New'){
		externalMethod = load("${BikewaleCI}" + "\\sonarqube-new\\SonarQube.groovy")
		externalMethod.RunAnalysis() 
	}
     //Call the method we defined in externalMethod.
}
