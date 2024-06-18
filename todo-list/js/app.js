(function(){
	
	//时间格式化
	Date.prototype.Format = function (fmt) { 
		var o = {
			"M+": this.getMonth() + 1, //月份 
			"d+": this.getDate(), //日 
			"h+": this.getHours(), //小时 
			"m+": this.getMinutes(), //分 
			"s+": this.getSeconds(), //秒 
			"q+": Math.floor((this.getMonth() + 3) / 3), //季度 
			"S": this.getMilliseconds() //毫秒 
		};
		if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
		for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
		return fmt;
	}
	
	
	
	//未完成的任务列表
	var newTaskList = document.getElementById("newTaskList");
	//文本输入框
	var input = document.getElementById("input");
	//已完成的任务数量
	var taskNum = document.getElementById("taskNum");
	//超链接
	var link = document.querySelector(".container");
	//已完成的任务列表
	var doneTaskList = document.getElementById("doneTaskList");


	//基础数据     JSON 对象 数组
	var tasks = null;
	var key = new Date().Format("yyyy-MM-dd");
	if(localStorage.getItem(key)) {
		tasks = JSON.parse(localStorage.getItem(key));
		console.log("from localStoage");
	}else{
		tasks = [];
	}
	

	
	
	//计算当前已完成的任务数量
	var finishNum = function(){
		var total = 0;
		for(var index in tasks) {
			if(tasks[index].done) {
				total++;
			}
		}
		return total;
	};
	
	
	
	//根据task的json对象 产生li元素并添加到未完成的列表中
	var renderTask = function(task){	
		var li = document.createElement("li"); //<li></li>
		var div = document.createElement("div"); //<div></div>
		var span = document.createElement("span"); //<span></span>
		span.setAttribute("class","checkbox");
		span.setAttribute("ref",task.id);
		
		var txt = document.createTextNode(task.todo); //xxxx
		
		div.appendChild(span);
		div.appendChild(txt);
		
		li.appendChild(div);
		
		if(!task.done) {
			if(newTaskList.childNodes.length == 0) {
				newTaskList.appendChild(li);
			} else {
				var firstLi = newTaskList.firstChild;
				newTaskList.insertBefore(li,firstLi);
			}
		} else {
			li.setAttribute("class","done");
			doneTaskList.appendChild(li);
			
			link.setAttribute("class","container");
			taskNum.innerText = finishNum();
		}
	};
	
	
	
	for(var index in tasks) {
		var task = tasks[index];
		renderTask(task);
	}
	
	
	//历史记录
	var storage=window.localStorage;
	var htmlarr = [];//按照日期倒序
	var html = '';
	 var reg = /^\d{4}-\d{2}-\d{2}$/;
	for(var si in storage){
		if(si != key && reg.test(si)){
			var html = '';
			html += '<div class="his_title">'+
						'<span class="his_date">'+si+'</span>'+
						'<div class="his_clear" ref="'+si+'">清除</div>'+
					'</div>'+
					'<ul class="list" id ="'+si+'">';
			var his_list = JSON.parse(storage[si]);
			for(var lii in his_list){
				
				if(his_list[lii].done){
					html +=	'<li class="done"><div><span ref="'+his_list[lii].id+'"></span>'+his_list[lii].todo+'</div></li>';
				}else{
					html +=	'<li><div><span ref="'+his_list[lii].id+'"></span>'+his_list[lii].todo+'</div></li>';
				}
			}
						
			html +=	'</ul>';
			htmlarr.push(html);
		}
	}
	//处理顺序问题
	html ='';
	for(var j = 0,len=htmlarr.length;j< len;j++){
		html += htmlarr.pop();
	}
	
	var history = document.querySelector(".history");
	if(html.length > 0){
		history.setAttribute("class","history");//覆盖hide
		
	}
	

	//文本框的keydown事件
	input.onkeydown = function(event){
		if(event.keyCode == 13) {
			var result = event.target.value;
			if(result.replace(/\s+/g, "") == ''){
				alert("请输入内容！！！");
				return;
			}
			//清空输入框的内容
			event.target.value = ""; 
			
			//计算新ID
			var taskid = new Date().getTime();//JSON.parse(localStorage.getItem("tasks")).length+1;
			
			var task = {id:taskid,todo:result,done:false};
			
			//保存到本地存储中
			tasks.push(task);
			localStorage.setItem(key,JSON.stringify(tasks));
			
			
			renderTask(task);
		}
	};
	
	//复选框(span)点击（事件委托）
	document.onclick = function(event){
		var t = event.target; // span
		
		if(t.getAttribute("class") == "checkbox") {
			var li = t.parentNode.parentNode;
			
			var id = t.getAttribute("ref");
			console.log("ID:" + id);
			
			if(!li.getAttribute("class")) {
				//未完成→已完成
				newTaskList.removeChild(li);
				li.setAttribute("class","done");
				doneTaskList.appendChild(li);
				
				taskNum.innerText = parseInt(taskNum.innerText) + 1;
				link.setAttribute("class","container");
				
				//保存到本地存储
				for(var index in tasks) {
					if(tasks[index].id == id) {
						tasks[index].done = true;
						break;
					}
				}
				localStorage.setItem(key,JSON.stringify(tasks));
				
				
			} else {
				doneTaskList.removeChild(li);
				li.setAttribute("class","");
				newTaskList.appendChild(li);
				
				taskNum.innerText = parseInt(taskNum.innerText) - 1;
				if(taskNum.innerText == "0") {
					link.setAttribute("class","container hide");
				}
				
				for(var index in tasks) {
					if(tasks[index].id == id) {
						tasks[index].done = false;
						break;
					}
				}
				localStorage.setItem(key,JSON.stringify(tasks));
				
				
			}
			
		
		}else if(t.getAttribute("class") == "his_clear"){
			var mydate = t.getAttribute("ref");
			localStorage.removeItem(mydate);
			console.log("remove ："+mydate);
			
			//1.刷新页面，模拟点击，使得“历史记录”展开 2.不刷新页面，删除对应的元素
			//第一种实现不了
			/*location.reload();
			var moni_open = document.querySelector(".his_open");
			moni_open.setAttribute("ref",false);
			moni_open.innerHTML = '历史记录(点击隐藏)';
			history_content.innerHTML = html;*/
			var content = t.parentNode.parentNode;
			content.removeChild(t.parentNode);
			content.removeChild(document.getElementById(mydate));
			
		}else if(t.getAttribute("class") == "his_open"){
			var flag = t.getAttribute("ref");
			var history_content = document.querySelector(".history_content");
			if(flag == "true"){//true,需要展开
				t.setAttribute("ref",false);
				t.innerHTML = '历史记录(点击隐藏)';
				history_content.innerHTML = html;
			}else{
				t.setAttribute("ref",true);
				t.innerHTML = '历史记录(点击展开)';
				history_content.innerHTML = "";
			}
		}
	};


})();
