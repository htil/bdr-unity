mergeInto(LibraryManager.library, {

  SaveTime: function (time) {
    var trial = localStorage.getItem('trial');
    var trialNum = 0;
    if (trial == null) {
      localStorage.setItem('trial', 1);
      trialNum = 1;
    }
    else {
      trialNum = parseInt(trial);
    }

    if(localStorage.getItem('time' + trialNum) == null) {
      localStorage.setItem('time' + trialNum, time);
      localStorage.setItem('trial', trialNum + 1);
    }
  },

  SetState: function (started, ended, restarted) {
    window.started = started; 
    window.ended = ended;
    window.restarted = restarted;
  },

});