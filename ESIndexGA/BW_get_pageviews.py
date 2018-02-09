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
import elasticsearch.exceptions

PROFILE_ID = "110715270"
INDEX_NAME = "bikewalekeywords"	#Live
ELASTIC_SEARCH_IP = '10.10.3.70'	#Live
ELASTIC_SEARCH_PORT = 9212			#Live
#INDEX_NAME = "bikewalekeywordsstgv1"	#Staging
#ELASTIC_SEARCH_IP = '10.10.3.70'		#Staging
#ELASTIC_SEARCH_PORT = 9211				#Staging
#INDEX_NAME = "11bwsrckeywordsv1"	#Local
#ELASTIC_SEARCH_IP = '172.16.0.11'	#Local
#ELASTIC_SEARCH_PORT = 9201			#Local
BW_HOSTURL = "https://www.bikewale.com"	#Live
#BW_HOSTURL = "https://staging.bikewale.com"	#Staging
#BW_HOSTURL = "http://webserver:9011"	#Local
current_directory = sys.path[0] + "/"
key_file_location = current_directory + 'GoogleKey.json' # + '/GoogleDFPProductionKey.p12'
DOC_TYPE = 'bikelist'
#BWOPR_HOSTURL = "http://localhost:9010" # Local
#BWOPR_HOSTURL = "http://bwoprst.bikewale.com" # Staging
BWOPR_HOSTURL = "https://opr.bikewale.com" # Production

def logf(msg):
	print "{} : {}".format(datetime.datetime.now(), msg)

def get_service(api_name, api_version, scope, key_file_location):

  credentials = ServiceAccountCredentials.from_json_keyfile_name(key_file_location,	scopes=scope)

  http = credentials.authorize(httplib2.Http())

  # Build the service object.
  service = build(api_name, api_version, http=http)

  return service

def getMakeModels(URL):
	response = requests.get(URL)
	data_json = response.text
	data = json.loads(data_json)
	return data

def saveModelsPageviews(URL, data, headers):
	response = requests.post(url = URL, data = data, headers=headers)
	data_json = response.text
	logf("{} > ({})".format(URL, response.status_code))
	data = json.loads(data_json)
	return data


def getGAData(service, profile_id, start_date, end_date, url):
	'''
		Function to get PageViews Data from GA
		@params
			service : service Object
			profile_id: the Profile_id for which data has to be fetched
			start_date: date from which data has to be fetched
			end_date: end date of data 
			url: 

 	'''
	ids = "ga:" + profile_id
	metrics = "ga:pageviews"
	filters = 'ga:pagePath=='+url
	data = service.data().ga().get(
		ids=ids, start_date=start_date, end_date=end_date, metrics=metrics
		,filters=filters).execute()
	return data["totalsForAllResults"]


def main():
	logf("Script execution started")
	
	# Define the Time Period 
	startdate = str(datetime.date.today()-datetime.timedelta(7))
	enddate = str(datetime.date.today()-datetime.timedelta(1))

	# # define Elastic Search Client object
	es = Elasticsearch(
    ELASTIC_SEARCH_IP,
    port=ELASTIC_SEARCH_PORT,
	)

	scope='https://www.googleapis.com/auth/analytics.readonly'

	# Get Services
	service = get_service('analytics', 'v3', scope, key_file_location)

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

		# Get Data From GA
		desktopData = getGAData(service,PROFILE_ID, startdate, enddate, "/"+url)
		mobileData = getGAData(service,PROFILE_ID, startdate, enddate, "/m/"+url)
		
		# compute total 
		data = int(desktopData['ga:pageviews']) + int(mobileData['ga:pageviews'])

		# Save pageviews with respective model id in a string which will pass on to api to save in database
		modelPageViews = modelPageViews + str(makeModel['ModelBase']['modelId']) + ":" + str(data) + ","
		
		# Fetch from ES
		Id = str(makeModel['MakeBase']['makeId']) +"_" + str(makeModel['ModelBase']['modelId'])

		try:			
			document = es.get(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id)['_source']
			logf("Document ID {} , Before Update Weight {} , After Update Weight {}".format(document['id'] , document['mm_suggest']['weight'], data))

			# Update Document Weight
			doc1 = {
					    "doc" : {
					        "mm_suggest" : {
					            "weight" : data
					        }
					    }
					}


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
		except:
			logf("Document {} not found in index ".format(Id))

	for i in range(len(Makes)):
		# Get Data From GA
		desktopData = getGAData(service,PROFILE_ID, startdate, enddate, "/"+Makes[i])
		mobileData = getGAData(service,PROFILE_ID, startdate, enddate, "/m/"+Makes[i])
		
		# compute total 
		data = int(desktopData['ga:pageviews']) + int(mobileData['ga:pageviews'])
		
		# Fetch from ES
		Id = str(MakeId[i]) +"_0"
		try:			
			document = es.get(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id)['_source']
			logf("Document ID {} , Before Update Weight {} , After Update Weight {}".format(document['id'] , document['mm_suggest']['weight'], data))
			
			# Update Document Weight
			document['mm_suggest']['weight'] = data

			doc1 = {
					    "doc" : {
					        "mm_suggest" : {
					            "weight" : data
					        }
					    }
					}

			es.update(index=INDEX_NAME, doc_type=DOC_TYPE, id= Id, body = doc1)

		except:
			logf("Document {} not found in index ".format(Id))

	# Modified By : Ashish Kamble on 1 Feb 2018	
	modelPageViews = modelPageViews[:-1] # Remove trailing comma

	# Send to model page views to api which will save data to database
	logf("ModelPageViews updation started")
	
	modelsPageviewsURL = BWOPR_HOSTURL + "/api/models/pageviews/"
	headers = {
		'Content-Type': "application/json",
		'Cache-Control': "no-cache"
	}
	saveModelsPageviews(modelsPageviewsURL, "'{}'".format(modelPageViews), headers)
	logf("API called using Model List: {}".format(modelPageViews))
	logf("ModelPageViews updation finished")

	logf("Script execution completed")

if __name__ =='__main__':
	main()
