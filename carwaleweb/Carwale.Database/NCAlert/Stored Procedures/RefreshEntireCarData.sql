IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCAlert].[RefreshEntireCarData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCAlert].[RefreshEntireCarData]
GO

	
----Created By: Manish Chourasiya on 20-05-2014 
----Description: For refreshing entire car data using in New car alert email.
--- Modified by: Manish on 01-07-2014 for considering all makes
-- Approved by Avishkar 01-07-2014, With (NoLock) is used, created Clustered Index IX_CW_NewCarShowroomPrices_Id
CREATE PROCEDURE [NCAlert].[RefreshEntireCarData]
AS 
BEGIN 

        TRUNCATE TABLE NCAlert.MatchedCarVersions;	
		TRUNCATE TABLE NCAlert.SimilarModelsForNewCarEmail;
		TRUNCATE TABLE NCAlert.NewCarMaster;
		TRUNCATE TABLE NCAlert.NewCarAlertEmailEntireCarData;

			INSERT INTO NCAlert.NewCarMaster													
											(MakeId,					
											MakeName,					
											ModelId,					
											ModelName,					
											ModelMaskingName,					
											VersionId,					
											VersionName,					
											ImageUrl,					
											ExShowroomPrice,					
											RTO,					
											Insurance,					
											DepotCharges)					
													
					SELECT CM.ID,											
						   CM.Name,											
						   CMO.ID,										
						   CMO.Name,										
						   CMO.MaskingName,										
						   CV.ID,										
						   CV.Name,										
						  CV.HOSTURL+CV.DirPath+ CV.largePic,										
						   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV.ID AND C.CityId=1 AND PQ_CategoryItem=2),										
						   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV.ID AND C.CityId=1 AND PQ_CategoryItem=3),										
						   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV.ID AND C.CityId=1 AND PQ_CategoryItem=5),										
						   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV.ID AND C.CityId=1 AND PQ_CategoryItem=26)										
					FROM CarMakes AS CM WITH (NOLOCK)											
					JOIN CarModels AS CMO WITH (NOLOCK) ON CM.Id=CMO.CarMakeId											
					JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId=CMO.ID											
					WHERE CM.IsDeleted=0											
					AND   CMO.IsDeleted=0											
					AND CV.IsDeleted=0											
					AND CV.NEW=1											
					AND CMO.NEW=1	;										
					--AND CM.ID IN (7,8,10);	---condition commented by Manish on 01-07-2014 										
													
  
						 WITH CTE1 AS													
									( SELECT  CV.ID PQVersionId,							
											CV1.ID  MTVersionId,						
											CM.ID   MTMakeId,						
											CM.Name MTMakeName,					
											CMO.ID  MTModelId,					
											CMO.Name MTModelName ,					
											CMO.MaskingName MTModelMaskingName,					
											CV1.Name MTVersionName,					
											 CV1.HOSTURL+CV1.DirPath+ CV1.largePic MTImageUrl,					
										   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV1.ID AND C.CityId=1 AND PQ_CategoryItem=2) MTShowroomPrice,					
										   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV1.ID AND C.CityId=1 AND PQ_CategoryItem=3) MTRTO,					
										   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV1.ID AND C.CityId=1 AND PQ_CategoryItem=5) MTInsurance,					
										   (SELECT PQ_CategoryItemValue FROM CW_NewCarShowroomPrices AS C WITH(NOLOCK) WHERE C.CarVersionId=CV1.ID AND C.CityId=1 AND PQ_CategoryItem=26) MTDepotCharges					
									FROM  CarVersions AS CV WITH (NOLOCK)						
									JOIN  CarVersions AS CV1 WITH (NOLOCK) ON CV.CarModelId=CV1.CarModelId						
									JOIN  CarModels AS CMO WITH (NOLOCK) ON CMO.Id=CV1.CarModelId						
									JOIN  CarMakes AS CM WITH (NOLOCK) ON CM.Id=CMO.CarMakeId						
									WHERE CM.IsDeleted=0						
									AND   CMO.IsDeleted=0						
									AND   CV.IsDeleted=0						
									AND   CV1.IsDeleted=0						
								--	AND   CM.ID IN (7,8,10)		  ---condition commented by Manish on 01-07-2014 						
									AND   CV.ID<>CV1.ID						
									AND   CV1.NEW=1						
								), CTE2 AS ( SELECT *,ROW_NUMBER() OVER (PARTITION BY  PQVersionId  ORDER BY ISNULL(MTShowroomPrice,99999999) ) MatchingOrder FROM CTE1 )							
													
							   INSERT INTO NCALERT.MatchedCarVersions													
										   (PQVersionId,					
											MTVersionId,				
											MTMakeId,				
											MTMakeName,				
											MTModelId,				
											MTModelName,				
											MTModelMaskingName,				
											MTVersionName,				
											MTImageUrl,				
											MTShowroomPrice,				
											MTRTO,				
											MTInsurance,				
											MTDepotCharges,				
											MatchingOrder)				
								SELECT     PQVersionId,							
											MTVersionId,				
											MTMakeId,				
											MTMakeName,				
											MTModelId,				
											MTModelName,				
											MTModelMaskingName,				
											MTVersionName,				
											MTImageUrl,				
											MTShowroomPrice,				
											MTRTO,				
											MTInsurance,				
											MTDepotCharges,				
											MatchingOrder				
									FROM CTE2 WHERE MatchingOrder<=3		;				
													
								
---------------------------------------------------------------------------------------------------------------									
	 DECLARE @VersionId INT,
	         @ModelIds  VARCHAR (100),
			 @CarModelId INT,  
			 @WhileLoopcontrol INT =1,
			 @WhileLoopCount INT 
	       
      DECLARE @TblVersions TABLE (TblId INT IDENTITY(1,1),VersionId INT ,ModelId INT )
	 
	 INSERT INTO @TblVersions (VersionId,ModelId)
	 SELECT VersionId,ModelId  
	 FROM   VWMMV 
	-- WHERE  MakeId IN (7,8,10)	---condition commented by Manish on 01-07-2014 			
	 
	 SELECT @WhileLoopCount=Count(*)
	 FROM @TblVersions

	 WHILE (@WhileLoopcontrol<=@WhileLoopCount)
	 BEGIN 

       SELECT @VersionId=VersionId,@CarModelId=ModelId 
	   FROM  @TblVersions
	   WHERE TblId=@WhileLoopcontrol;


	   SELECT @ModelIds =SimilarModels
	   FROM  SimilarCarModels WITH (NOLOCK) 
	   WHERE ModelId=@CarModelId;

	   WITH CTE AS (SELECT @VersionId PQVersionId,
									       CMO.ID SmModelId,
										   CMK.id SmMakeId,
										   CMK.Name SmMakeName,
										   CMO.Name SmModelName,
										   CMO.MaskingName SmModelMaskingName ,
										   CMO.HostUrl+'/cars/'+CMO.LargePic SmImageUrl,
										   CMO.MinPrice SmMinPrice,
										   ROW_NUMBER() OVER (  ORDER BY csv.id,ISNULL(CMO.MinPrice,99999999) ) SimilarPriorityOrder
									FROM fnSplitCSVValuesWithIdentity(@ModelIds)  AS CSV 	
									JOIN  CarModels  AS CMO WITH (NOLOCK) ON CMO.ID=CSV.ListMember
									JOIN  CarMakes  AS CMK WITH (NOLOCK) ON CMK.ID=CMO.CarMakeId
									
					 )
	  INSERT INTO NCAlert.SimilarModelsForNewCarEmail												
										   (PQVersionId,				
											SmModelId,			
											SmMakeId,			
											SmMakeName,			
											SmModelName,			
											SmModelMaskingName,			
											SmImageUrl,			
											SmMinPrice,			
											SimilarPriorityOrder)
											SELECT * FROM CTE WHERE SimilarPriorityOrder<=4

												
						
						
						SET @WhileLoopcontrol=@WhileLoopcontrol+1;			
								
		END 					 						
													
													
------------------------------------------------------------------------------------------------------------------------													
					INSERT INTO NCAlert.NewCarAlertEmailEntireCarData													
																		(MakeId,
																		MakeName,
																		ModelId,
																		ModelName,
																		ModelMaskingName,
																		VersionId,
																		VersionName,
																		ImageUrl,
																		ExShowroomPrice,
																		RTO,
																		Insurance,
																		DepotCharges)
																SELECT 													
																		MakeId,
																		MakeName,
																		ModelId,
																		ModelName,
																		ModelMaskingName,
																		VersionId,
																		VersionName,
																		ImageUrl,
																		ExShowroomPrice,
																		RTO,
																		Insurance,
																		DepotCharges
																 FROM NCAlert.NewCarMaster	WITH (NOLOCK) 													
													
													
					UPDATE A SET    MT1VersionId=MTVersionId,													
									MT1MakeId=MTMakeId,									
									MT1MakeName=MTMakeName,									
									MT1ModelId=MTModelId,									
									MT1ModelName=MTModelName,									
									MT1ModelMaskingName=MTModelMaskingName,									
									MT1VersionName=MTVersionName,									
									MT1ImageUrl=MTImageUrl,									
									MT1ShowroomPrice=MTShowroomPrice,									
									MT1RTO=MTRTO,									
									MT1Insurance=MTInsurance,									
									MT1DepotCharges=MTDepotCharges									
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A	WITH (NOLOCK) 													
					JOIN NCAlert.MatchedCarVersions AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.MatchingOrder=1													
													
					UPDATE A SET    MT2VersionId=MTVersionId,													
									MT2MakeId=MTMakeId,									
									MT2MakeName=MTMakeName,									
									MT2ModelId=MTModelId,									
									MT2ModelName=MTModelName,									
									MT2ModelMaskingName=MTModelMaskingName,									
									MT2VersionName=MTVersionName,									
									MT2ImageUrl=MTImageUrl,									
									MT2ShowroomPrice=MTShowroomPrice,									
									MT2RTO=MTRTO,									
									MT2Insurance=MTInsurance,									
									MT2DepotCharges=MTDepotCharges									
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.MatchedCarVersions AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.MatchingOrder=2													
													
													
					UPDATE A SET    MT3VersionId=MTVersionId,													
									MT3MakeId=MTMakeId,									
									MT3MakeName=MTMakeName,									
									MT3ModelId=MTModelId,									
									MT3ModelName=MTModelName,									
									MT3ModelMaskingName=MTModelMaskingName,									
									MT3VersionName=MTVersionName,									
									MT3ImageUrl=MTImageUrl,									
									MT3ShowroomPrice=MTShowroomPrice,									
									MT3RTO=MTRTO,									
									MT3Insurance=MTInsurance,									
									MT3DepotCharges=MTDepotCharges									
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.MatchedCarVersions AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.MatchingOrder=3													
													
													
													
													
					UPDATE A SET   Sm1ModelId=SmModelId,													
								   Sm1MakeId=SmMakeId,													
								   Sm1MakeName=SmMakeName,													
								   Sm1ModelName=SmModelName,													
								   Sm1ModelMaskingName=SmModelMaskingName,													
								   Sm1ImageUrl=SmImageUrl,													
									Sm1MinPrice=SmMinPrice													
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.SimilarModelsForNewCarEmail  AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.SimilarPriorityOrder=1													
													
													
					UPDATE A SET   Sm2ModelId=SmModelId,													
								   Sm2MakeId=SmMakeId,													
								   Sm2MakeName=SmMakeName,													
								   Sm2ModelName=SmModelName,													
								   Sm2ModelMaskingName=SmModelMaskingName,													
								   Sm2ImageUrl=SmImageUrl,													
									Sm2MinPrice=SmMinPrice													
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.SimilarModelsForNewCarEmail  AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.SimilarPriorityOrder=2													
													
													
													
					UPDATE A SET   Sm3ModelId=SmModelId,													
								   Sm3MakeId=SmMakeId,													
								   Sm3MakeName=SmMakeName,													
								   Sm3ModelName=SmModelName,													
								   Sm3ModelMaskingName=SmModelMaskingName,													
								   Sm3ImageUrl=SmImageUrl,													
									Sm3MinPrice=SmMinPrice													
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.SimilarModelsForNewCarEmail  AS B  WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.SimilarPriorityOrder=3													
													
													
													
					UPDATE A SET   Sm4ModelId=SmModelId,													
								   Sm4MakeId=SmMakeId,													
								   Sm4MakeName=SmMakeName,													
								   Sm4ModelName=SmModelName,													
								   Sm4ModelMaskingName=SmModelMaskingName,													
								   Sm4ImageUrl=SmImageUrl,													
									Sm4MinPrice=SmMinPrice													
					FROM NCAlert.NewCarAlertEmailEntireCarData AS  A WITH (NOLOCK) 														
					JOIN NCAlert.SimilarModelsForNewCarEmail  AS B WITH (NOLOCK) 	 ON A.VersionId=B.PQVersionId													
					WHERE  B.SimilarPriorityOrder=4													


END
