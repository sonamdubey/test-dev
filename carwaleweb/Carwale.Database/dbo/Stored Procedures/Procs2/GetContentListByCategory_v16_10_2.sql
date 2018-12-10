IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentListByCategory_v16_10_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentListByCategory_v16_10_2]
GO

	
--================================================================
-- author: natesh kumar on 28/8/14 
--description: gets the article list based on categoryid/categoryids with applicationid, categoryids list , startindex, endindex, makeid and modelid as input parameter
-- modified by natesh on 4/11/14 for recordcount
-- modified by natesh kumar checking isdeleted flag on join on 26/11/14
-- modified by natesh kumar on 1/12/14 for null and 0 for isdeleted
-- modified by : shalini nair on 17/02/15 added left join on cbs and cs for applicationid=2
-- modified by satish sharma on 23/jul/2015, image dynamic resize revamp
-- modified by sachin bharti on 22/sep/2015, added category masking name 
-- modifier by sachin bharti on 14/10/2015, added categorymaskingname for application id 2
-- exec [dbo].[getcontentlistbycategory_v15.9.4] 1,12,1,10
-- modified by rakesh yadav on 17-12-2015 commented sticky cases since it is not using as on now.and also made chages regarding optimization of the code only for carwale query.
-- modified by manish on 21-12-2015 rearrangening the join in carwale query for optimization purpose
-- modified by sumit kate on 16-02-2016 returnt the isfeatured for applicationid = 2
-- am added 04-05-2016 to order by basic id
-- optimised sp for performance - satish sharma on 6th may 2016
-- exec [GetContentListByCategory_v16_5_3] 1, '2,8', 1,10,9, 408
-- Modified By Rakesh Yadav on 22 Jun 2016 to show sponsored news Bridgestone (Sponsored)(1551),
-- Modified By Rakesh Yadav on 22 Jun 2016 to show sponsored news BMW (Sponsored)(1561),
-- Modified By Rakesh Yadav on 8 Sep 2016 to show sponsored news Mahindra Electric (Sponsored)(1571),
-- Modified By Rohan S on 6th OCT 2016 , removed sponsored article query added by rakesh, added query to get count by category, optimized query for make model filter
--================================================================
CREATE PROCEDURE [dbo].[GetContentListByCategory_v16_10_2] 
--DECLARE
	@applicationid TINYINT --= 1
	,@categorylist VARCHAR(50) --= '1,2,6,8,12,19,22'
	,@startindex INT --= 1
	,@endindex INT --= 18
	,@makeid INT = NULL--7
	,@modelid INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@applicationid = 1)
	BEGIN
		IF ((@makeid IS NULL OR @makeid < 1) AND (@modelid IS NULL OR @modelid < 1))
		BEGIN		
			SELECT *
			FROM (
				SELECT cb.id AS basicid
					,cb.categoryid AS categoryid
					,cb.title AS title
					,cb.url AS articleurl
					,CB.displaydate AS displaydate
					,cb.authorname AS authorname
					,cb.authorid
					,cb.description
					,CB.publisheddate
					,cb.VIEWS AS VIEWS
					,(isnull(cb.issticky, 0)) AS issticky
					,(isnull(cb.isfeatured, 0)) AS isfeatured
					,ca.maskingname AS authormaskingname
					,cb.hosturl AS hosturl
					,cb.mainimagepath AS orginalimgpath
					,(isnull(cb.facebookcommentcount, 0)) AS facebookcommentcount
					,cat.NAME AS CategoryMaskingName
					,row_number() OVER (
						ORDER BY cb.displaydate DESC
						) AS row_no
				FROM con_editcms_basic cb WITH (NOLOCK)
				INNER JOIN Con_EditCms_Category cat WITH (NOLOCK) ON cat.Id = cb.CategoryId
				INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid
				WHERE cb.isactive = 1
					AND cb.ispublished = 1
					AND cb.applicationid = 1
					AND cb.categoryid IN (
						SELECT listmember
						FROM fnsplitcsvvalueswithidentity(@categorylist)
						)
					AND cb.displaydate <= getdate()
					AND ca.Authorid <> 1571
				) CTE
			WHERE row_no BETWEEN @startindex
					AND @endindex
			ORDER BY row_no

			SELECT cb.CategoryId,count(cb.id) AS recordcount
			FROM con_editcms_basic cb WITH (NOLOCK)
			WHERE cb.isactive = 1
				AND cb.ispublished = 1
				AND cb.applicationid = 1
				AND (cb.categoryid IN (
					SELECT listmember
					FROM fnsplitcsvvalueswithidentity(@categorylist)
					) OR cb.categoryid IN (1,2,6,8,12,19,22))
				AND cb.displaydate <= getdate()
			GROUP BY cb.CategoryId
		END
		ELSE -- Query with Make, Model filter-----------------------
		BEGIN
			SELECT *
			FROM (
				SELECT cb.id AS basicid
					,cb.categoryid AS categoryid
					,cb.title AS title
					,cb.url AS articleurl
					,displaydate AS displaydate
					,cb.authorname AS authorname
					,cb.authorid
					,cb.description
					,publisheddate
					,cb.VIEWS AS VIEWS
					,(isnull(cb.issticky, 0)) AS issticky
					,(isnull(cb.isfeatured, 0)) AS isfeatured
					,ca.maskingname AS authormaskingname
					,cb.hosturl AS hosturl
					,cb.mainimagepath AS orginalimgpath
					,(isnull(cb.facebookcommentcount, 0)) AS facebookcommentcount
					,cat.NAME AS CategoryMaskingName
					,row_number() OVER (
						ORDER BY displaydate DESC
						) AS row_no
				FROM con_editcms_basic cb WITH (NOLOCK)
				INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid
				INNER JOIN (select distinct(BasicId) from Con_EditCms_Cars tag WITH (NOLOCK) where (@makeid IS NULL OR tag.MakeId = @makeid) and (@modelid IS NULL OR tag.ModelId = @modelid)) tag on tag.BasicId = cb.id
				INNER JOIN Con_EditCms_Category cat WITH (NOLOCK) ON cat.Id = cb.CategoryId
				WHERE cb.isactive = 1
					AND cb.ispublished = 1
					AND cb.applicationid = 1
					AND cb.categoryid IN (
						SELECT listmember
						FROM fnsplitcsvvalueswithidentity(@categorylist)
						)
					AND cb.displaydate <= getdate()
				) CTE
			WHERE row_no BETWEEN @startindex
					AND @endindex
			ORDER BY row_no

			-- record count query
			SELECT cb.CategoryId,count(cb.id) AS recordcount
			FROM con_editcms_basic cb WITH (NOLOCK)
			INNER JOIN (select distinct(BasicId) from Con_EditCms_Cars tag WITH (NOLOCK) where (@makeid IS NULL OR tag.MakeId = @makeid) and (@modelid IS NULL OR tag.ModelId = @modelid)) tag on tag.BasicId = cb.id
			WHERE cb.isactive = 1
				AND cb.ispublished = 1
				AND cb.applicationid = 1
				AND (cb.categoryid IN (
					SELECT listmember
					FROM fnsplitcsvvalueswithidentity(@categorylist)
					) OR cb.categoryid IN (1,2,6,8,12,19,22))
				AND cb.displaydate <= getdate()
				GROUP BY cb.CategoryId
		END
	END
	ELSE
		IF (@applicationid = 2)
		BEGIN
			-- sticky query.
			--(
			--		select *
			--		from (
			--			select distinct top 1000  cb.id as basicid
			--				,cb.categoryid as categoryid
			--				,cb.title as title
			--				,cb.url as articleurl
			--				,displaydate as displaydate
			--				,cb.authorname as authorname
			--				,ca.maskingname as authormaskingname
			--				,cb.description as description
			--				,publisheddate
			--				,cb.views as views
			--				,(isnull(spc.facebookcommentcount, 0)) as facebookcommentcount
			--				,(isnull(cb.issticky, 0)) as issticky
			--				,(isnull(cb.isfeatured,0)) as isfeatured
			--				,cei.hosturl as hosturl
			--				--,cei.imagepathlarge as largepicurl
			--				--,cei.imagepaththumbnail as smallpicurl
			--				,cei.originalimgpath as orginalimgpath
			--				,cs.name as subcategoryname
			--				--,'' as makeid, '' as modelid, '' as makename, '' as modelname, '' as modelmaskingname 	
			--				,m.id as makeid
			--				,mo.id as modelid
			--				,m.name as makename
			--				,mo.name as modelname
			--				,mo.maskingname as modelmaskingname
			--				,cc.categorymaskingname
			--				,row_number() over (
			--					partition by cb.id order by displaydate desc
			--					) as row_no
			--				,'1' as row_num
			--			from con_editcms_basic as cb with (nolock)
			--			inner join con_editcms_author ca with (nolock) on ca.authorid = cb.authorid
			--			left join con_editcms_images cei with (nolock) on cei.basicid = cb.id
			--				and cei.ismainimage = 1
			--				and cei.isactive = 1
			--			left join socialpluginscount spc with (nolock) on spc.typeid = cb.id
			--			left join con_editcms_cars c with (nolock) on c.basicid = cb.id
			--				and c.isactive = 1
			--			left join bikemodels mo with (nolock) on mo.id = c.modelid
			--			left join bikemakes m with (nolock) on m.id = c.makeid
			--			-- added by : shalini nair added left join cbs and cs
			--			left join con_editcms_basicsubcategories cbs with (nolock) on cb.id = cbs.basicid
			--			left join con_editcms_subcategories cs with (nolock) on cs.id = cbs.subcategoryid
			--			left join con_editcms_category cc with (nolock) on cc.id = cb.categoryid
			--			where cb.applicationid = @applicationid
			--				and cb.categoryid in (
			--					select listmember
			--					from fnsplitcsvvalueswithidentity(@categorylist)
			--					)
			--				and cb.isactive = 1
			--				and cb.ispublished = 1
			--				and isnull(mo.isdeleted, 0) = 0
			--				and isnull(m.isdeleted, 0) = 0
			--				and issticky = 1
			--				and (
			--					cast(getdate() as date) between cast(stickyfromdate as date)
			--						and cast(stickytodate as date)
			--					)
			--			) as cte
			--		where row_no = 1
			--		)
			--union all
			(
					-- normal query
					SELECT *
					FROM (
						SELECT *
							,row_number() OVER (
								ORDER BY displaydate DESC
								) AS row_num
						FROM (
							SELECT cb.id AS basicid
								,cb.categoryid AS categoryid
								,cb.title AS title
								,cb.url AS articleurl
								,displaydate AS displaydate
								,cb.authorname AS authorname
								,ca.maskingname AS authormaskingname
								,cb.description AS description
								,publisheddate
								,cb.VIEWS AS VIEWS
								,(isnull(spc.facebookcommentcount, 0)) AS facebookcommentcount
								,(isnull(cb.issticky, 0)) AS issticky
								,(isnull(cb.isfeatured, 0)) AS isfeatured
								,cei.hosturl AS hosturl
								--,cei.imagepathlarge as largepicurl
								--,cei.imagepaththumbnail as smallpicurl
								,cei.originalimgpath AS orginalimgpath
								,cs.NAME AS subcategoryname
								,m.id AS makeid
								,mo.id AS modelid
								,m.NAME AS makename
								,mo.NAME AS modelname
								,mo.maskingname AS modelmaskingname
								,cc.categorymaskingname
								,row_number() OVER (
									PARTITION BY cb.id ORDER BY displaydate DESC
									) AS row_no
							FROM con_editcms_basic cb WITH (NOLOCK)
							INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid
							LEFT JOIN con_editcms_images cei WITH (NOLOCK) ON cei.basicid = cb.id
								AND cei.ismainimage = 1
								AND cei.isactive = 1
							LEFT JOIN socialpluginscount spc WITH (NOLOCK) ON spc.typeid = cb.id
							LEFT JOIN con_editcms_cars c WITH (NOLOCK) ON c.basicid = cb.id
								AND c.isactive = 1
							LEFT JOIN bikemodels mo WITH (NOLOCK) ON mo.id = c.modelid
							LEFT JOIN bikemakes m WITH (NOLOCK) ON m.id = c.makeid
							LEFT JOIN con_editcms_basicsubcategories cbs WITH (NOLOCK) ON cb.id = cbs.basicid
							LEFT JOIN con_editcms_subcategories cs WITH (NOLOCK) ON cs.id = cbs.subcategoryid
							LEFT JOIN con_editcms_category cc WITH (NOLOCK) ON cc.id = cb.categoryid
							WHERE cb.categoryid IN (
									SELECT listmember
									FROM fnsplitcsvvalueswithidentity(@categorylist)
									)
								AND cb.applicationid = @applicationid
								AND cb.isactive = 1
								AND cb.ispublished = 1
								AND isnull(mo.isdeleted, 0) = 0
								AND isnull(m.isdeleted, 0) = 0
								AND cb.displaydate <= getdate()
								AND (
									issticky IS NULL
									OR issticky = 0
									OR cast(stickytodate AS DATE) < cast(getdate() AS DATE)
									)
								AND (
									@makeid IS NULL
									OR m.id = @makeid
									)
								AND (
									@modelid IS NULL
									OR mo.id = @modelid
									)
							) AS cte1
						WHERE row_no = 1
						) cte
					WHERE row_num BETWEEN @startindex
							AND @endindex
					)
			ORDER BY row_num

			-- record count query
			SELECT count(DISTINCT cb.id) AS recordcount
			FROM con_editcms_basic cb WITH (NOLOCK)
			INNER JOIN con_editcms_author ca WITH (NOLOCK) ON ca.authorid = cb.authorid
			LEFT JOIN con_editcms_cars c WITH (NOLOCK) ON c.basicid = cb.id
				AND c.isactive = 1
			LEFT JOIN bikemodels mo WITH (NOLOCK) ON mo.id = c.modelid
			LEFT JOIN bikemakes m WITH (NOLOCK) ON m.id = c.makeid
			WHERE cb.categoryid IN (
					SELECT listmember
					FROM fnsplitcsvvalueswithidentity(@categorylist)
					)
				AND cb.ispublished = 1
				AND cb.applicationid = @applicationid
				AND cb.isactive = 1
				AND isnull(mo.isdeleted, 0) = 0
				AND isnull(m.isdeleted, 0) = 0
				AND cb.displaydate <= getdate()
				AND (
					issticky IS NULL
					OR issticky = 0
					OR cast(stickytodate AS DATE) < cast(getdate() AS DATE)
					)
				AND (
					@makeid IS NULL
					OR m.id = @makeid
					)
				AND (
					@modelid IS NULL
					OR mo.id = @modelid
					)
		END
END

