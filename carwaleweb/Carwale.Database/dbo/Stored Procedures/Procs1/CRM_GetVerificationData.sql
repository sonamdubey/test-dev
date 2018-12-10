IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetVerificationData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetVerificationData]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 1 April 2013
-- Description : Get Individual verification report
-- =============================================

CREATE PROCEDURE [dbo].[CRM_GetVerificationData]

    @groupIdCross NUMERIC(18,0),
	@groupId NUMERIC(18,0),
	@grpUser VARCHAR(50),
	@toDate DATETIME,
	@fromDate DATETIME
	AS
	
	BEGIN
		  DECLARE @TempModel Table( ModelId NUMERIC)
          INSERT INTO @TempModel
						  SELECT DISTINCT FR.ModelId AS ModelId  FROM CRM_ADM_FLCRules AS FR WITH(NOLOCK)
						  INNER JOIN  CarVersions AS CV  WITH (NOLOCK) ON CV.CarModelId=FR.ModelId
						  WHERE   (@groupIdCross IS NULL OR FR.GroupId  = @groupIdCross) AND FR.ModelId <> -1
						  UNION 
						  SELECT DISTINCT CM.Id AS ModelId 
						  FROM CRM_ADM_FLCRules AS FR WITH (NOLOCK)
						  INNER JOIN  CarModels AS CM WITH (NOLOCK) ON FR.MakeId = CM.CarMakeId AND CM.IsDeleted=0
						  INNER JOIN  CarVersions AS CV  WITH (NOLOCK) ON CV.CarModelId=CM.Id
						  WHERE  (@groupIdCross IS NULL OR FR.GroupId  = @groupIdCross) AND FR.ModelId=-1
						
		  IF(@grpUser IS NOT NULL)
			  BEGIN			
		  
				SELECT ISNULL(COUNT(DISTINCT L.id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId, L.LeadStageId,  L.Owner, L.LeadStatusId, ISNULL(L.LeadVerifier,-1) AS LeadVerifier  
						  FROM CRM_Leads L WITH(NOLOCK)  
						  LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier  OR OU.Id = L.Owner
						  WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND (@grpUser IS NULL OR OU.Id IN(SELECT * FROM list_to_tbl(@grpUser)))
						  GROUP BY  L.LeadStageId, L.Owner, L.LeadStatusId,OU.UserName, L.LeadVerifier, OU.Id ORDER BY OU.UserName
				SELECT ISNULL(COUNT(DISTINCT L.id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId,
						  CVL.CBDId, CVL.DealerId, VM.CarModelId, TM.ModelId AS Model
						  FROM CRM_Leads L WITH(NOLOCK)  
						  LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier 
						  LEFT JOIN CRM_VerificationLog AS CVL WITH (NOLOCK) ON CVL.LeadId=L.ID
						  LEFT JOIN CRM_CarBasicData AS CBD  WITH (NOLOCK) ON CVL.CBDId = CBD.ID
						  LEFT JOIN CarVersions AS VM  WITH (NOLOCK) ON CBD.VersionId= VM.Id AND VM.IsDeleted=0
						  LEFT JOIN @TempModel AS TM  ON TM.ModelId = VM.CarModelId
						  WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND  L.LeadStatusId = 2  AND (@grpUser IS NULL OR OU.Id IN(SELECT * FROM list_to_tbl(@grpUser)))
						  GROUP BY  OU.UserName, OU.Id ,CVL.CBDId, CVL.DealerId, VM.CarModelId,TM.ModelId ORDER BY OU.UserName
				SELECT ISNULL(COUNT(DISTINCT CC.Id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId
						 FROM CRM_Leads L WITH(NOLOCK) 
						 INNER JOIN CRM_Calls CC ON CC.LeadId = L.ID
						 LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier 
						 WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND L.LeadStatusId = 2  AND CC.CallType IN(1,2)  AND (@grpUser IS NULL OR OU.Id IN(SELECT * FROM list_to_tbl(@grpUser)))
						 GROUP BY OU.UserName, OU.Id ORDER BY OU.UserName
				
				END
				
		   ELSE
		   
		      BEGIN
		       		  
				SELECT ISNULL(COUNT(DISTINCT L.id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId, L.LeadStageId,  L.Owner, L.LeadStatusId, ISNULL(L.LeadVerifier,-1) AS LeadVerifier  
						  FROM CRM_Leads L WITH(NOLOCK)  
						  LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier 
						  WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND (@groupId IS NULL OR L.GroupId  = @groupId) 
						  GROUP BY  L.LeadStageId, L.Owner, L.LeadStatusId,OU.UserName, L.LeadVerifier, OU.Id ORDER BY OU.UserName
				SELECT ISNULL(COUNT(DISTINCT L.id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId,
						  CVL.CBDId, CVL.DealerId, VM.CarModelId, TM.ModelId AS Model
						  FROM CRM_Leads L WITH(NOLOCK)  
						  LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier 
						  LEFT JOIN CRM_VerificationLog AS CVL WITH (NOLOCK) ON CVL.LeadId=L.ID
						  LEFT JOIN CRM_CarBasicData AS CBD  WITH (NOLOCK) ON CVL.CBDId = CBD.ID
						  LEFT JOIN CarVersions AS VM  WITH (NOLOCK) ON CBD.VersionId= VM.Id AND VM.IsDeleted=0
						  LEFT JOIN @TempModel AS TM  ON TM.ModelId = VM.CarModelId
						  WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND (@groupId IS NULL OR  L.GroupId  = @groupId) AND L.LeadStatusId = 2 
						  GROUP BY  OU.UserName, OU.Id ,CVL.CBDId, CVL.DealerId, VM.CarModelId,TM.ModelId ORDER BY OU.UserName
				SELECT ISNULL(COUNT(DISTINCT CC.Id),0) AS TotalLeads, ISNULL(OU.UserName, 'NA') AS UserName, ISNULL(OU.Id, -1) AS UserId
						 FROM CRM_Leads L WITH(NOLOCK) 
						 INNER JOIN CRM_Calls CC ON CC.LeadId = L.ID
						 LEFT JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id=L.LeadVerifier 
						 WHERE L.CreatedOnDatePart BETWEEN @fromDate AND @toDate AND (@groupId IS NULL OR  L.GroupId  = @groupId) AND L.LeadStatusId = 2  AND CC.CallType IN(1,2)
						 GROUP BY OU.UserName, OU.Id ORDER BY OU.UserName
		        
		        END	
		END