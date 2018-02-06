import httplib2

from oauth2client.service_account import ServiceAccountCredentials
from oauth2client import client
from oauth2client import file
from oauth2client import tools

from apiclient.discovery import build
import requests
import json
import datetime

import pika
from elasticsearch import Elasticsearch
import sys
import os
from array import array
import subprocess
import logging
import elasticsearch.exceptions

LOGGER_NAME = 'BWUpdatePageViewsLogger'
PROFILE_ID = "110715270"
#INDEX_NAME = "bikewalekeywords"	#Live
#ELASTIC_SEARCH_IP = '10.10.3.70'	#Live
#ELASTIC_SEARCH_PORT = 9212			#Live
#INDEX_NAME = "bikewalekeywordsstgv1"	#Staging
#ELASTIC_SEARCH_IP = '10.10.3.70'		#Staging
#ELASTIC_SEARCH_PORT = 9211				#Staging
INDEX_NAME = "11bwsrckeywordsv1"	#Local
ELASTIC_SEARCH_IP = '172.16.0.11'	#Local
ELASTIC_SEARCH_PORT = 9201			#Local
#BW_HOSTURL = "https://www.bikewale.com"	#Live
#BW_HOSTURL = "https://staging.bikewale.com:10007"	#Staging
BW_HOSTURL = "http://localhost:9011"	#Local
key_file_location = 'GoogleKey.p12' # + '/GoogleDFPProductionKey.p12'
DOC_TYPE = 'bikelist'
BWOPR_HOSTURL = "http://localhost:9010" # Local
#BWOPR_HOSTURL = "http://bwoprst.bikewale.com" # Staging
#BWOPR_HOSTURL = "https://opr.bikewale.com" # Production

def get_Logger(loggerName,fileName): 
	logger = logging.getLogger(loggerName)
	logger.setLevel(logging.INFO)
	fh = logging.FileHandler(fileName)
	formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')
	fh.setFormatter(formatter)
	logger.addHandler(fh)
	return logger

# logger = get_Logger(LOGGER_NAME,LOG_FILE) #'/home/cassuser/PageViewUpdatePythonScript/UpdatePageViewsScript.log')
current_directory = sys.path[0] + "/"
logger = get_Logger(LOGGER_NAME,current_directory+"BWUpdatePageViewsScript.log")


def get_service(api_name, api_version, scope, key_file_location,
				service_account_email):

  f = open(key_file_location, 'rb')
  key = f.read()
  f.close()

  credentials = ServiceAccountCredentials._from_p12_keyfile_contents(service_account_email, key,
	scopes=scope)

  http = credentials.authorize(httplib2.Http())

  # Build the service object.
  service = build(api_name, api_version, http=http)

  return service

def getMakeModels(URL):
	response = requests.get(URL)
	data_json = response.text
	data = json.loads(data_json)
	return data

def saveModelsPageviews(URL, data):
	response = requests.post(url = URL, data = data)
	data_json = response.text
	data = json.loads(data_json)
	return data


def getDesktopData(service, profile_id, start_date, end_date, url):
	'''
		Function to get Desktop PageViews Data from GA
		@params
			service : service Object
			profile_id: the Profile_id for which data has to be fetched
			start_date: date from which data has to be fetched
			end_date: end date of data 
			url: 

 	'''
	ids = "ga:" + profile_id
	metrics = "ga:pageviews"
	filters = 'ga:pagePath==/'+url #/m/marutisuzuki-cars/default.aspx'
	data = service.data().ga().get(
		ids=ids, start_date=start_date, end_date=end_date, metrics=metrics
		,filters=filters).execute()
	return data["totalsForAllResults"]


def getMobileData(service, profile_id, start_date, end_date, url):
	# Function to get Mobile PageViews Data from GA
	ids = "ga:" + profile_id
	metrics = "ga:pageviews"
	filters = 'ga:pagePath==/m/'+url #/m/marutisuzuki-cars/default.aspx'
	data = service.data().ga().get(
		ids=ids, start_date=start_date, end_date=end_date, metrics=metrics
		,filters=filters).execute()
	return data["totalsForAllResults"]



def main():
	logger.info("started at " + str(datetime.date.today()))
	# Queue Name
	#Queue_name = "RabbitMq-BWAUTOSUGGEST-Queue"
	# Define the Time Period 
	startdate = str(datetime.date.today()-datetime.timedelta(7))
	enddate = str(datetime.date.today()-datetime.timedelta(1))

	# # define Elastic Search Client object
	es = Elasticsearch(
    ELASTIC_SEARCH_IP,
    port=ELASTIC_SEARCH_PORT,
	)

	service_account_email = '701834893970-6864575bc9csg290qv0142kt5e2n6asd@developer.gserviceaccount.com'
	application_name = 'New Srevice Account'
	scope='https://www.googleapis.com/auth/analytics.readonly'

	# Get Services
	service = get_service('analytics', 'v3', scope, key_file_location,service_account_email)

	# Get make and model URLS
	makeModelUrl = BW_HOSTURL + "/api/model/all/new/"
	makeModelUrlUpcoming = BW_HOSTURL + "/api/model/all/upcoming/"
	
	MakeModels = getMakeModels(makeModelUrl) + getMakeModels(makeModelUrlUpcoming)

	# # For each MakeModel 
	# # create URL and fetch from GA
	# # fetch from ES
	# # Push to Queue
	Makes =[]
	MakeId =[]
	roundrobinflag = 0
	modelPageViews = "" # Variable to store model pageviews
	for makeModel in MakeModels:
		
		# Create page URL 
		url =  makeModel['MakeBase']['maskingName'] + "-bikes/" + makeModel['ModelBase']['maskingName'] + "/"
		# Creating MakeUrl
		makeurl = makeModel['MakeBase']['maskingName'] + "-bikes/"
		if Makes.count(makeurl) == 0:
			Makes.append(makeurl)
			MakeId.append(makeModel['MakeBase']['makeId'])
			print makeurl

		# Get Data From GA
		desktopData = getDesktopData(service,PROFILE_ID, startdate, enddate, url)
		mobileData = getMobileData(service,PROFILE_ID, startdate, enddate, url)
		
		# compute total 
		data = int(desktopData['ga:pageviews']) + int(mobileData['ga:pageviews'])

		# Save pageviews with respective model id in a string which will pass on to api to save in database
		modelPageViews = modelPageViews + str(makeModel['ModelBase']['modelId']) + ":" + str(data) + ","
		
		# Fetch from ES
		Id = str(makeModel['MakeBase']['makeId']) +"_" + str(makeModel['ModelBase']['modelId'])
		print Id
		print url
		print 
		try:			
			document = es.get(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id)['_source']
			print "Before Update Weight " , document['id'] , document['mm_suggest']['weight']
		# Update Document Weight
			document['mm_suggest']['weight'] = data
			print "After Update Weight " , document['id'] , document['mm_suggest']['weight']
			#doc = '{"OperationType" : "Create","docs" :' + str(document) + '}'

			#print document

			#doc1 = str(document).replace("u'","\"").replace("'","\"")

			#print doc1
			
			#doc1 = "\"doc\" : \{\"mm_suggest\" : \{\"weight\" : 2001\}\}"
			doc1 = {
					    "doc" : {
					        "mm_suggest" : {
					            "weight" : data
					        }
					    }
					}

			print doc1

			es.update(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id, body = doc1)

			# Push it into Queue
			#fields = {}
			#fields['content'] = 'json'
			#if roundrobinflag == 0:			
			#	channel.basic_publish(exchange='',
		    #                  routing_key=QUEUE_NAME,
		    #                  body=doc1,
		    #                  properties = pika.BasicProperties(headers = fields))
			#	roundrobinflag = 1
			#else :
			#	channel.basic_publish(exchange='',
		    #                  routing_key=QUEUE_NAME,
		    #                  body=doc1,
		    #                  properties = pika.BasicProperties(headers = fields))
			#	roundrobinflag = 0
			# print doc1
			logger.info("Processed  ID " + Id)
		except:
			logger.error(Id + "Document Not Found in Index")

	for i in range(len(Makes)):
		# Get Data From GA
		desktopData = getDesktopData(service,PROFILE_ID, startdate, enddate, Makes[i])
		mobileData = getMobileData(service,PROFILE_ID, startdate, enddate, Makes[i])
		
		# compute total 
		data = int(desktopData['ga:pageviews']) + int(mobileData['ga:pageviews'])
		
		# Fetch from ES
		Id = str(MakeId[i]) +"_0"
		try:			
			document = es.get(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id)['_source']
		# Update Document Weight
			document['mm_suggest']['weight'] = data

			#print document
		
			#doc = '{"OperationType" : "Create","docs" :' + str(document) + '}'
			#doc1 = str(document).replace("u'","\"").replace("'","\"")

			#print doc1

			doc1 = {
					    "doc" : {
					        "mm_suggest" : {
					            "weight" : data
					        }
					    }
					}

			print doc1
			
			es.update(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id, body = doc1)

			# Update the document from the script itself

			# Push it into Queue
			#fields = {}
			#fields['content'] = 'json'
			#channel.basic_publish(exchange='',
	        #              routing_key=QUEUE_NAME,
	        #              body=doc1,
	        #              properties = pika.BasicProperties(headers = fields))
			logger.info("Processed  ID " + Id)
		except:
			logger.error(Id + "Document Not Found in Index")

	# Modified By : Ashish Kamble on 1 Feb 2018	
	modelPageViews = modelPageViews[:-1] # Remove trailing comma

	# Send to model page views to api which will save data to database
	# print modelPageViews
	logger.info("modelPageViews updation started")
	
	modelsPageviewsURL = BWOPR_HOSTURL + "/api/models/pageviews/"	
	saveModelsPageviews(modelsPageviewsURL, modelPageViews)	
	#print modelPageViews
	
	logger.info("modelPageViews updation finished")

	logger.info("Completed at " + str(datetime.date.today()))
	# print doc1


if __name__ =='__main__':
	main()

