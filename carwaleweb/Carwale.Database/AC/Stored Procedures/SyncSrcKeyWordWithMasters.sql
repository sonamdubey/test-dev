IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[SyncSrcKeyWordWithMasters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[SyncSrcKeyWordWithMasters]
GO

	--CREATED BY: Manish Chourasiya on 13-08-2014
--Description: This sp will sync the data of Src_Keywords table with Masters.
--Modified by Manish on 26-08-2015 for not updating and deleting manual entries.
CREATE PROCEDURE [AC].[SyncSrcKeyWordWithMasters]
AS
BEGIN 

    TRUNCATE TABLE [AC].[IntermediateSRC_Keywords];

	-----------CARMAKE
    INSERT INTO  [AC].[IntermediateSRC_Keywords]
                                                (
												 [Name],
												 [KeywordTypeId],
												 [ReferenceId],
												 [DisplayName],
												 [Value],
												 [IsAutomated],
												 IsNew,
												 IsUsed
												)
                                     SELECT [ac].[RSC_ExceptSpaces](CMA.Name),
	                                         1,
	                                         CMA.ID,
	                                         CMA.Name,
	                                         ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100)),
	                                         1,
	                                         CMA.New,
	                                         CMA.Used
                                      FROM CarMakes CMA WITH(NOLOCK) 
									  WHERE CMA.IsDeleted = 0;

               
			  

            
			------------CARMAKEMODEL
	      INSERT INTO  [AC].[IntermediateSRC_Keywords] 
                                                        (
														  [Name],
														  [KeywordTypeId],
														  [ReferenceId],
														  [DisplayName],
														  [IsNew],
														  [IsUsed],
														  [Value],
														  [IsAutomated])
                          SELECT                    [ac].[RSC_ExceptSpaces](CMA.Name + ' ' + CMO.Name),
	                                                     4,
	                                                     CMO.ID,
	                                                     CMA.Name + ' ' + CMO.Name,
	                                                     CMO.New,
	                                                     CMO.Used,
	                                                     ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100)),
	                                                     1
                                          FROM CarModels CMO WITH(NOLOCK)
                                          INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
                                          WHERE CMO.IsDeleted = 0
										  AND CMA.IsDeleted=0;


                        ------------CARMODEL
                       INSERT INTO  [AC].[IntermediateSRC_Keywords] 
					                                              (
																	[Name],
																	[KeywordTypeId],
																	[ReferenceId],
																	[DisplayName],
																	[IsNew],
																	[IsUsed],
																	[Value],
																	[IsAutomated]
																  )
									SELECT  [ac].[RSC_ExceptSpaces](CMO.Name),
										                           2,
										                           CMO.ID,
										                           CMA.Name + ' ' + CMO.Name,
										                           CMO.New,
										                           CMO.Used,
										                           ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100)),
										                           1
									FROM CarModels CMO WITH(NOLOCK)
									INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
									AND CMO.IsDeleted=0
									AND CMA.IsDeleted=0


                         --CARMAKEMODELVERSION
						INSERT INTO  [AC].[IntermediateSRC_Keywords]
									                                 ([Name],
																	  [KeywordTypeId],
																	  [ReferenceId],
																	  [DisplayName],
																	  [IsNew],
																	  [IsUsed],
																	  [Value],
																	  [IsAutomated]
																	 )
						              SELECT          [ac].[RSC_ExceptSpaces](CMA.Name + ' ' + CMO.Name + ' ' + CV.Name),
															5,
															CV.ID,
															CMA.Name + ' ' + CMO.Name + ' ' + CV.Name DN,
															CV.New,
															CV.Used,
							                              ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100)),
							                              1
											FROM CarModels CMO WITH(NOLOCK)
											INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
											INNER JOIN CarVersions CV WITH(NOLOCK) ON CMO.ID = CV.CarModelId
											WHERE CMO.IsDeleted = 0 
											AND CV.IsDeleted = 0
											AND CMA.IsDeleted=0

                    ------------------CARMODELVERSION
                INSERT INTO  [AC].[IntermediateSRC_Keywords] 
				                              (
											   [Name],
											   [KeywordTypeId],
											   [ReferenceId],
											   [DisplayName],
											   [IsNew],
											   [IsUsed],
											   [Value],
											   [IsAutomated]
											  )
								SELECT [ac].[RSC_ExceptSpaces](CMO.Name + ' ' + CV.Name),
									   6,
									   CV.ID,
									   CMA.Name + ' ' + CMO.Name + ' ' + CV.Name DN,
									   CV.New,
									   CV.Used,
									   ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100)),
									   1
								FROM CarModels CMO WITH(NOLOCK)
								INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
								INNER JOIN CarVersions CV WITH(NOLOCK) ON CMO.ID = CV.CarModelId
								WHERE CMO.IsDeleted = 0 
								AND CV.IsDeleted = 0
								AND CMA.IsDeleted=0
----------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------

-------Sync of CarVersions , CarModels and CarMake masters

IF         (   (SELECT COUNT(DISTINCT KeywordTypeId) FROM  [AC].[IntermediateSRC_Keywords] WITH (NOLOCK)) =5
            ) 

      BEGIN
				MERGE AC.SRC_Keywords  AS SK
											USING (  SELECT * FROM   
													 [AC].[IntermediateSRC_Keywords] WITH (NOLOCK)
													 WHERE KeyWordTypeId IN (1,2,3,4,5,6)
												  ) AS  ISK
									   ON  SK.KeywordTypeId = ISK.KeywordTypeId
									   AND SK.ReferenceId=ISK.ReferenceId
									  -- AND SK.IsAutomated=1
				WHEN MATCHED AND SK.IsAutomated=1 THEN UPDATE SET   SK.Name=ISK.Name,
																	SK.DisplayName=ISK.DisplayName,
																	SK.IsNew=ISK.IsNew,
																	SK.IsUsed=ISK.IsUsed,
																	SK.Value=ISK.Value
									
						WHEN NOT MATCHED  BY TARGET  THEN
										INSERT(Name,
												KeywordTypeId,
												ReferenceId,
												DisplayName,
												IsNew,
												IsUsed,
												IsPriceExist,
												Value,
												IsAutomated)
										VALUES (ISK.Name,
												ISK.KeywordTypeId,
												ISK.ReferenceId,
												ISK.DisplayName,
												ISK.IsNew,
												ISK.IsUsed,
												ISK.IsPriceExist,
												ISK.Value,
												ISK.IsAutomated)
						WHEN NOT MATCHED BY SOURCE  AND SK.KeyWordTypeId IN (1,2,3,4,5,6)
						                            --AND SK.IsAutomated=1
												 THEN DELETE;
              
			  END;

			

			------------------------------------------------------------
			
			
				------------------Sync of City table -------------------------------------------------------------------------------------------------------
				------------------------------case when rename city name  or delete city---------------------------------------------------------------	
								DELETE  
								FROM ac.SRC_Keywords
								WHERE keywordtypeid=7
								AND (   ReferenceId in (SELECT id   
													     FROM Cities WITH (NOLOCK)
													     WHERE IsDeleted=1
												        )
										OR ReferenceId IN
										( SELECT AffectedId 
										  FROM CarWaleMasterDataLogs 
										  WHERE TableName ='cities' 
										  AND ColumnName='name'
                                          AND Remarks='Record Updated'
                                          AND OldValue<>NewValue
										)
								   )
        

-------------------------------------------------------------------------------------------------------------------------
 ---------CITY
              INSERT INTO  [AC].[SRC_Keywords]
			                                                (
															 [Name],
															 [KeywordTypeId],
															 [ReferenceId],
															 [DisplayName],
															 [Value],
															 [IsAutomated]
															)
						 SELECT                      [ac].[RSC_ExceptSpaces] (C.Name),
												            7,
												            C.ID,
												            C.Name,
												            C.Id,
												            1
											FROM Cities C WITH (NOLOCK) 
											WHERE C.IsDeleted = 0
											AND ID NOT IN (SELECT ReferenceID 
											                 FROM AC.SRC_Keywords WITH (NOLOCK)
															 WHERE KeywordTypeId=7
														  );

-------------------------------------------------------------------------------------------------------------------------


			DECLARE @CityTable TABLE (ID INT IDENTITY(1,1) , CityId INT, CityName VARCHAR(100))

			INSERT INTO @CityTable (CityId,CityName)
			SELECT id as cityid, name   
			FROM  Cities  WITH (NOLOCK)
			WHERE isdeleted=0 
			AND name like '% %'
			AND Id IN ( Select AffectedId 
			            FROM CarWaleMasterDataLogs WITH (NOLOCK)
			            WHERE TableName='Cities' 
						AND CreatedOn >'2014-07-25 00:00:00.000'  
						AND Remarks='Record Inserted' 
					   )

			DECLARE @WhileLoopControl int=1
			DECLARE @TotalWhileLoopCount int
			DECLARE @CityName VARCHAR(100)
			DECLARE @Separator_position INT
			DECLARE @CityId INT
			DECLARE @AlterName VARCHAR(100)
			DECLARE @NoOfWord  SMALLINT =0
			DECLARE @ActualCityName VARCHAR(100)

			SELECT @TotalWhileLoopCount =COUNT(ID) FROM @CityTable

  WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
  BEGIN
         SELECT @CityId=CityId,@CityName=CityName ,@ActualCityName=CityName FROM  @CityTable  WHERE ID=@WhileLoopControl;

		 SET @CityName=@CityName +' '

		 SET @NoOfWord=0

		 WHILE   PATINDEX('% %', @CityName) <> 0  
		 BEGIN 
		 

		  SELECT  @Separator_position = PATINDEX('% %',@CityName)  
          SELECT  @AlterName = LEFT(@CityName, @Separator_position - 1)  
			
		    
			
			--PRINT @AlterName

			IF @NoOfWord>0 and @AlterName<>'new' and @AlterName<>'and' and @AlterName<>'with octroi'
			BEGIN 
			  INSERT INTO AC.SRC_Keywords
			                               (Name,
											KeywordTypeId,
											ReferenceId,
											DisplayName,
											IsNew,
											IsUsed,
											IsPriceExist,
											Value,
											IsAutomated
											)
							SELECT  LOWER(@AlterName),
							        7,
									@CityId,
									@ActualCityName,
									0,
									0,
									0,
									@CityId,
									0




			END 
			SELECT  @CityName = STUFF(@CityName, 1, @Separator_position, '')
			SET @NoOfWord=@NoOfWord+1;

		 END 
		 
		
		SET  @WhileLoopControl=@WhileLoopControl+1


  END

  
-------------------------------------------------------------------------------------------------------------------------

	END 
