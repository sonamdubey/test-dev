IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAllMMV]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAllMMV]
GO

	
--Tejashree Patil on 7 Nov 2014
--Description: To Get All MMV (based on Application)
--[TC_GetAllMMV] -1,160,1,0,1,-1
--Modified By : Tejashree Patil on 14 Nov 2014, Checked Make id exist in TC_DealerMakes or not.
--Modified by : Kritika Choudhary on 15th Sep 2016, added order by in select query
--==================================================
CREATE PROCEDURE [dbo].[TC_GetAllMMV] 
	@MakeId INT=-1, -- Default -1 
	@ModelId INT=-1, -- Default -1 
    @ApplicationId INT=1, -- Default 1 for CarWale 
    @NeedListOfMakes BIT=0, -- Default 1 for getting All Makes
	@IsUsed BIT = 1, -- Default 1 for USED Vehicles
	@DealerId INT = -1 -- DealerId used for NEW Car/Bike dealers only and -1 for used Car by default SEND ONLY IN CASE OF MAKE.
AS
BEGIN
	DECLARE @IsNew BIT = NULL
	IF(@IsUsed <= 0 OR @IsUsed IS NULL)
	BEGIN
		SET @IsNew = 1
		SET @IsUsed = NULL
	END

	IF EXISTS(SELECT TOP 1 MakeId FROM TC_DealerMakes WITH(NOLOCK) WHERE DealerId=@DealerId)
	SELECT Text,Value FROM
		(SELECT	DISTINCT V.Make Text,V.MakeID Value
		FROM	dbo.vwAllMMV V
				LEFT JOIN TC_DealerMakes DM WITH(NOLOCK) ON DM.MakeId = V.MakeId
		WHERE	(ApplicationId=@ApplicationId
				AND @NeedListOfMakes=1
				AND (@IsUsed IS NULL OR Used = @IsUsed )
					AND (@IsNew IS NULL OR New = @IsNew )
					AND DM.DealerId=@DealerId)
       
				--OR (V.ModelFuturistic = 1 OR V.VersionFuturistic = 1 )
				
		UNION ALL

		SELECT	DISTINCT Model Text,ModelID Value
		FROM	dbo.vwAllMMV V
		WHERE	ApplicationId=@ApplicationId
				AND (MakeId=@MakeId)
				AND (((@IsUsed IS NULL OR Used = @IsUsed )
					AND (@IsNew IS NULL OR New = @IsNew ))
					OR (ModelFuturistic = 1))
		
				--OR (V.ModelFuturistic = 1 OR V.VersionFuturistic = 1 )
	
		UNION ALL
	
		SELECT	DISTINCT Version Text, VersionId Value 
		FROM	dbo.vwAllMMV V
		WHERE	ApplicationId=@ApplicationId
				AND (ModelId=@ModelId)
				AND (((@IsUsed IS NULL OR Used = @IsUsed )
					AND (@IsNew IS NULL OR New = @IsNew ))
					OR (ModelFuturistic = 1)))Data
		ORDER BY Text
	ELSE
	SELECT Text,Value FROM
	    (SELECT	DISTINCT V.Make Text,V.MakeID Value
		FROM	dbo.vwAllMMV V
		WHERE	(ApplicationId=@ApplicationId
				AND @NeedListOfMakes=1
				AND (@IsUsed IS NULL OR Used = @IsUsed )
				AND (@IsNew IS NULL OR New = @IsNew ))
				--OR (V.ModelFuturistic = 1 OR V.VersionFuturistic = 1 )
	    
		
		UNION ALL

		SELECT	DISTINCT Model Text,ModelID Value
		FROM	dbo.vwAllMMV V
		WHERE	ApplicationId=@ApplicationId
				AND (MakeId=@MakeId)
				AND (((@IsUsed IS NULL OR Used = @IsUsed )
					AND (@IsNew IS NULL OR New = @IsNew ))
					OR (ModelFuturistic = 1))
				--OR (V.ModelFuturistic = 1 OR V.VersionFuturistic = 1 )
	    
		UNION ALL
	
		SELECT	DISTINCT Version Text, VersionId Value 
		FROM	dbo.vwAllMMV V
		WHERE	ApplicationId=@ApplicationId
				AND (ModelId=@ModelId)
				AND (((@IsUsed IS NULL OR Used = @IsUsed )
					AND (@IsNew IS NULL OR New = @IsNew ))
					OR (ModelFuturistic = 1)))Data
		ORDER BY Text
END
