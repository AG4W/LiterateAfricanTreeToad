const section1 = document.getElementById('section01')

function show_hide(){
    var x = document.getElementById("section1");
    if(x.style.display === "none"){
        section1.classList.remove('hide-section1')
        x.style.display = "block";
    }else{
        section1.classList.add('hide-section1')
        x.style.display = "none";
    }

}