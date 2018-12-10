IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AutoSuggestMyTask]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AutoSuggestMyTask]
GO

	
-- Author		:	Vivek Gupta
-- Create date	:	06-05-2013  
-- Description	:	It fills Auto suggests in My Task's Search Box 
-- EXEC [TC_AutoSuggestMyTask] 5,'Hon',1,2
-- Modifeid By : umesh on 29 june 2013 for selecting source via dealer wise
-- Modified By Vivek Gupta, Added with(nolock) in queries of conditions
-- Modified By Vivek Gupta, on 02-07-2014, Added filtertype=5 for inquiryType
-- Modified By: Ashwini Dhamankar on Nov 8,2014 Changed text New Car to New Vehicle
-- Modified By Vivek Gupta, on 23-03-2015, Added filtertype=6 for Lead Priority
-- Modified By Afrose on 10th June, 2015, changed inquiry source filter
-- Modified By Nilima More On 24th December,2015 (Fetching makeid,versionid,modelid) 
-- EXEC TC_AutoSuggestMyTask 5,'Hon',0,7
-- Modifed By : Suresh Prajapati on 28th June, 2016
-- Description : Refered TC_TaskLists table for customer details fetch
-- Modified By : Ashwini Dhamankar on July 25,2016 (Fetch Dealer Makes and details for NCD dealer)
-- Modified By : Khushaboo Patil on 23 Aug 2016 add parameter businessTypeid
-- =============================================     
CREATE PROCEDURE [dbo].[TC_AutoSuggestMyTask]
	-- Add the parameters for the stored procedure here     
	@BranchId INT
	,@SearchText VARCHAR(50)
	,@SearchType TINYINT
	,@FilterType TINYINT
	,@BusinessTypeId TINYINT = 3
AS
BEGIN
	DECLARE @ApplicationId AS TINYINT

	SET @ApplicationId = (
			SELECT ApplicationId
			FROM Dealers WITH (NOLOCK)
			WHERE ID = @BranchId
			)

	IF (@SearchType = 0) -- For Customer Name, Cars, UserName,Email, And Source
		BEGIN
			IF (@FilterType = 1) -- For CustomerName,Email
				BEGIN
					IF EXISTS (
							SELECT TOP 1 CustomerName
							FROM TC_TaskLists TL WITH (NOLOCK)
							WHERE CustomerName LIKE @SearchText + '%'
								AND BranchId = @BranchId AND TL.TC_BusinessTypeId = @BusinessTypeId
							) --For CustomerName
						BEGIN
							SELECT DISTINCT C.CustomerName AS AutoSuggest
							FROM TC_TaskLists AS C WITH (NOLOCK)
							WHERE C.BranchId = @BranchId
								-- AND C.IsleadActive = 1
								AND C.CustomerName LIKE @SearchText + '%' AND C.TC_BusinessTypeId = @BusinessTypeId
							ORDER BY C.CustomerName ASC
						END
					ELSE
						IF EXISTS (
								SELECT TOP 1 CustomerEmail AS Email
								FROM TC_TaskLists TL WITH (NOLOCK)
								WHERE CustomerEmail LIKE @SearchText + '%'
									AND BranchId = @BranchId AND TL.TC_BusinessTypeId = @BusinessTypeId
								) ---For Email Only
							BEGIN
								SELECT DISTINCT C.CustomerEmail AS AutoSuggest
								FROM TC_TaskLists AS C WITH (NOLOCK)
								WHERE C.BranchId = @BranchId
									-- AND C.IsleadActive = 1
									AND C.CustomerEmail LIKE @SearchText + '%' AND C.TC_BusinessTypeId = @BusinessTypeId
								ORDER BY C.CustomerEmail ASC
							END
				END
			ELSE IF (@FilterType = 2) --for Car Details
				BEGIN
					SELECT DISTINCT VW.Car AS AutoSuggest
						,vw.MakeId AS makeId
						,vw.VersionId AS versionId
						,vw.ModelId AS modelId
					FROM vwAllMMV vw WITH (NOLOCK)
					WHERE ApplicationId = @ApplicationId
						AND CAR LIKE '%' + @SearchText + '%'
				END
			ELSE IF(@FilterType = 7)   --NCD Dealer Car Details  --Added by : Ashwini Dhamankar on Jul 25,2016
				BEGIN
					SELECT  VW.Car AS AutoSuggest
					,vw.MakeId AS makeId
					,vw.VersionId AS versionId
					,vw.ModelId AS modelId
					FROM TC_DealerMakes DM WITH(NOLOCK) 
					INNER JOIN vwAllMMV vw WITH(NOLOCK) ON vw.MakeId = DM.MakeId
					WHERE ApplicationId = @ApplicationId
					AND CAR LIKE '%' + @SearchText + '%'
					AND DM.DealerId = @BranchId
				END	
			ELSE IF(@FilterType = 8)   --All NCD Cars Details  --Added by : Ashwini Dhamankar on Jul 25,2016
				BEGIN
					SELECT  VW.Car AS AutoSuggest
					,vw.MakeId AS makeId
					,vw.VersionId AS versionId
					,vw.ModelId AS modelId
					FROM vwAllMMV vw WITH(NOLOCK) 
					WHERE ApplicationId = @ApplicationId AND vw.IsModelNew = 1 AND vw.IsVerionNew = 1 AND CAR LIKE '%' + @SearchText + '%'
				END	
			ELSE IF (@FilterType = 3
						AND @ApplicationId = 1
						) --for Carwale source
				BEGIN
						SELECT DISTINCT SOURCE AS AutoSuggest
						FROM TC_InquirySource WITH (NOLOCK)
						WHERE (
								MakeId IS NULL
								OR MakeId IN (
									SELECT MakeId
									FROM TC_DealerMakes WITH (NOLOCK)
									WHERE DealerId = @BranchId
									)
								)
							AND IsActive = 1
							AND IsVisibleCW = 1
							AND SOURCE LIKE @SearchText + '%'
						ORDER BY SOURCE ASC
					END
			ELSE IF (
							@FilterType = 3
							AND @ApplicationId = 2
							) --for Bikewale source
				BEGIN
						SELECT DISTINCT SOURCE AS AutoSuggest
						FROM TC_InquirySource WITH (NOLOCK)
						WHERE (
								MakeId IS NULL
								OR MakeId IN (
									SELECT MakeId
									FROM TC_DealerMakes WITH (NOLOCK)
									WHERE DealerId = @BranchId
									)
								)
							AND IsActive = 1
							AND IsVisibleBW = 1
							AND SOURCE LIKE @SearchText + '%'
						ORDER BY SOURCE ASC
				END
			ELSE IF (@FilterType = 4) --for Lead Owner Name
				BEGIN
					SELECT DISTINCT TU.UserName AS AutoSuggest
					FROM TC_Users AS TU WITH (NOLOCK)
					WHERE TU.BranchId = @BranchId
						AND TU.IsActive = 1
						AND TU.UserName LIKE @SearchText + '%'
					ORDER BY TU.UserName ASC
				END
			ELSE IF (@FilterType = 5) --for Inquiry Type
				BEGIN
					DECLARE @DealerTypeId SMALLINT = NULL

					SELECT @DealerTypeId = TC_DealerTypeId
					FROM Dealers WITH (NOLOCK)
					WHERE ID = @BranchId

					IF (@DealerTypeId = 1)
						SELECT ListMember AS AutoSuggest
						FROM [dbo].[fnSplitCSVValuesWithIdentity]('Buyer,Seller')
						WHERE ListMember LIKE @SearchText + '%'
					ELSE IF (@DealerTypeId = 2)
						SELECT ListMember AS AutoSuggest
						FROM [dbo].[fnSplitCSVValuesWithIdentity]('New Vehicle')
						WHERE ListMember LIKE @SearchText + '%' --Modified By: Ashwini Dhamankar on Nov 8,2014 Changed text New Car to New Vehicle
					ELSE IF (@DealerTypeId = 3)
						SELECT ListMember AS AutoSuggest
						FROM [dbo].[fnSplitCSVValuesWithIdentity]('Seller,Buyer,New Vehicle')
						WHERE ListMember LIKE @SearchText + '%' --Modified By: Ashwini Dhamankar on Nov 8,2014 Changed text New Car to New Vehicle
				END
			ELSE IF (@FilterType = 6) --for Inquiry Type
				BEGIN
					SELECT ListMember AS AutoSuggest
					FROM [dbo].[fnSplitCSVValuesWithIdentity]('Fresh Leads,Pending Leads,Follow Up Leads')
					WHERE ListMember LIKE '%' + @SearchText + '%'
				END
							
		END
	ELSE IF (@SearchType = 1) --- For Mobile No. Only
		BEGIN
			SELECT DISTINCT C.CustomerMobile AS AutoSuggest
			FROM TC_TaskLists AS C WITH (NOLOCK)
			WHERE C.BranchId = @BranchId
				-- AND C.IsleadActive = 1
				AND C.CustomerMobile LIKE @SearchText + '%' AND C.TC_BusinessTypeId = @BusinessTypeId
			ORDER BY C.CustomerMobile ASC
		END
END


